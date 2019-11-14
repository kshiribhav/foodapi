using MongoDB.Bson.Serialization.Attributes;

namespace FoodAPI.Models
{
    public class CustomerReview
    {
        [BsonElement("username")]
        public string Username{ get; set; }

        [BsonElement("rating")]
        public double Rating { get; set; }

        [BsonElement("comments")]
        public string Comments { get; set; }

        [BsonElement("restaurantId")]
        public string RestaurantId { get; set; }
    }
}
