using FoodAPI.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FoodAPI.Infrastructure
{
    public class FoodContext
    {
        private IConfiguration config;
        private IMongoDatabase mongoDb;

        public FoodContext(IConfiguration configuration)
        {
            config = configuration;
            var connectionString = config.GetValue<string>("MongoSettings:ConnectionString");

            MongoClientSettings mongoSettings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            mongoSettings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };

            MongoClient mongoClient = new MongoClient(mongoSettings);
            if (mongoClient != null)
            {
                mongoDb = mongoClient.GetDatabase(config.GetValue<string>("MongoSettings:Database"));
            }
        }

        public IMongoCollection<RestaurantItem> Restaurants
        {
            get
            {
                return mongoDb.GetCollection<RestaurantItem>("Restaurants");
            }
        }
    }
}
