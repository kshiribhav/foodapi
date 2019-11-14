using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAPI.Infrastructure;
using FoodAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using FoodAPI.Helper;

namespace FoodAPI.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class FoodApiController : ControllerBase
    {
        private IConfiguration config;
        private FoodContext foodContext;

        public FoodApiController(IConfiguration configuration, FoodContext context)
        {
            config = configuration;
            foodContext = context;
        }

        [HttpGet("", Name = "GetRestaurants")]
        [AllowAnonymous]
        public async Task<ActionResult<List<RestaurantItem>>> GetRestaurants()
        {
            var result = await foodContext.Restaurants.FindAsync<RestaurantItem>(FilterDefinition<RestaurantItem>.Empty);
            return result.ToList();
        }

        [HttpGet("{id}", Name = "GetRestaurantById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<ActionResult<RestaurantItem>> GetRestaurantById(string id)
        {
            var builder = Builders<RestaurantItem>.Filter;
            var filter = builder.Eq("Id", id);
            var result = await foodContext.Restaurants.FindAsync(filter);
            var item = result.FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(item);
            }
        }

        //[Authorize(Roles = "hadmin")]
        [HttpPost("addRestaurant", Name = "AddRestaurant")]
        public ActionResult<RestaurantItem> AddRestaurant()
        {
            var imageName = SaveImageToCloudAsync(Request.Form.Files[0]).GetAwaiter().GetResult();
            var restaurantItem = new RestaurantItem()
            {
                Name = Request.Form["name"],
                Address = Request.Form["address"],
                Description = Request.Form["description"],
                IsPureVeg = Convert.ToBoolean(Request.Form["isPureVeg"]),
                Location = Request.Form["location"],
                RestaurantImgUrl = imageName,
                Branches = new List<string>(),
                FoodItems = new List<FoodItem>()
            };

            foodContext.Restaurants.InsertOne(restaurantItem);
            return restaurantItem;
        }

        //[Authorize(Roles = "hadmin")]
        [HttpPost("addFoodItem", Name = "AddFoodItem")]
        public ActionResult<FoodItem> AddFoodItem()
        {
            var imageName = SaveImageToCloudAsync(Request.Form.Files[0]).GetAwaiter().GetResult();
            var foodItem = new FoodItem()
            {
                Name = Request.Form["name"],
                Description = Request.Form["description"],
                FoodImageUrl = imageName,
                IsAvailable = Convert.ToBoolean(Request.Form["isAvailable"]),
                Price = decimal.Parse(Request.Form["price"]),
                RestaurantId = Request.Form["restaurantId"],
                FoodCategory = Request.Form["foodCategory"]
            };

            var filter = Builders<RestaurantItem>.Filter.Eq(e => e.Id, foodItem.RestaurantId);
            var update = Builders<RestaurantItem>.Update.Push<FoodItem>(e => e.FoodItems, foodItem);
            foodContext.Restaurants.FindOneAndUpdateAsync(filter, update);

            return foodItem;
        }

        [HttpPost("addReview", Name = "AddReview")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<CustomerReview>> AddCustomerReview(CustomerReview customerReview)
        {
            TryValidateModel(customerReview);
            if (ModelState.IsValid)
            {
                var filter = Builders<RestaurantItem>.Filter.Eq(e => e.Id, customerReview.RestaurantId);
                var result = await foodContext.Restaurants.FindAsync(filter);
                var avgRating = (result.FirstOrDefault().Ratings + customerReview.Rating) / 2;
                var update = Builders<RestaurantItem>.Update.Push<CustomerReview>(e => e.CustomerReviews, customerReview)
                    .Set(z => z.Ratings, avgRating);

                await foodContext.Restaurants.FindOneAndUpdateAsync(filter, update);
                return Created("", customerReview);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [NonAction]
        private async Task<string> SaveImageToCloudAsync(IFormFile image)
        {
            var imageName = $"{Guid.NewGuid()}_{image.FileName}";
            var tempFile = Path.GetTempFileName();
            using (FileStream fs = new FileStream(tempFile, FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }
            var imageFile = Path.Combine(Path.GetDirectoryName(tempFile), imageName);
            System.IO.File.Move(tempFile, imageFile);
            StorageHelper helper = new StorageHelper
            {
                BlobConnectionString = config.GetConnectionString("BlobConnection")
            };
            var fileUri = await helper.UploadFileBlobAsync(imageFile, "Images");
            System.IO.File.Delete(tempFile);
            return fileUri;
        }

        [NonAction]
        private string SaveImageToLocal(IFormFile image)
        {
            var imageName = $"{Guid.NewGuid()}_{image.FileName}";

            var dirName = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            var filePath = Path.Combine(dirName, imageName);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fs);
            }

            return $"/Images/{imageName}";
        }
    }
}