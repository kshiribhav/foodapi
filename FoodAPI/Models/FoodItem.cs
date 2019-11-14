using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace FoodAPI.Models
{
    public class FoodItem
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("restaurantId")]
        public string RestaurantId { get; set; }

        [BsonElement("foodCategory")]
        public string FoodCategory { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("isAvailable")]
        public bool IsAvailable { get; set; }

        [BsonElement("foodImageUrl")]
        public string FoodImageUrl { get; set; }
    }
}
