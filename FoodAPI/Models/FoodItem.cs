namespace FoodAPI.Models
{
    public class FoodItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string RestaurantId { get; set; }

        public string FoodCategory { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public string FoodImageUrl { get; set; }
    }
}
