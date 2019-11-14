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
        public string Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public bool IsPureVeg { get; set; }

        public double Ratings { get; set; }

        public string RestaurantImgUrl { get; set; }

        public List<string> Branches { get; set; }

        public List<FoodItem> FoodItems { get; set; }

        public List<CustomerReview> CustomerReviews { get; set; }
    }
}
