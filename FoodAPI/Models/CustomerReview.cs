namespace FoodAPI.Models
{
    public class CustomerReview
    {
        public string Username{ get; set; }

        public double Rating { get; set; }

        public string Comments { get; set; }

        public string RestaurantId { get; set; }
    }
}
