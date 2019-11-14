using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;

namespace FoodAPI.Models
{
    public class RestaurantItem
    {
        public RestaurantItem()
        {
            Branches = new List<string>();
            FoodItems = new List<FoodItem>();
            CustomerReviews = new List<CustomerReview>();
        }

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("isPureVeg")]
        public bool IsPureVeg { get; set; }

        [BsonElement("ratings")]
        public double Ratings { get; set; }

        [BsonElement("restaurantImgUrl")]
        public string RestaurantImgUrl { get; set; }

        [BsonElement("branches")]
        public List<string> Branches { get; set; }

        [BsonElement("foodItems")]
        public List<FoodItem> FoodItems { get; set; }

        [BsonElement("customerReviews")]
        public List<CustomerReview> CustomerReviews { get; set; }
    }
}
