using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB
{
    class Program
    {
        static void Main(string[] args)
        {

            // http://media.mongodb.org/zips.json


            var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            
            IMongoDatabase db = dbClient.GetDatabase("OTUS");
            var users = db.GetCollection<User>("Users");

            users.InsertOne(new User
            {
                age = 43,
                user_name = "Zaza",
                _id = Guid.NewGuid().ToString().ToLower(),
                company = new Company
                {
                    name = "Bueng",
                    startwork = DateTime.Now
                }

            });
            var indexKeysDefinition = Builders<User>.IndexKeys.Ascending(x => x.user_name);
             users.Indexes.CreateOne(new CreateIndexModel<User>(indexKeysDefinition));
            var u = users.Find<User>(x => x.company.startwork >= new DateTime(2002, 1, 1)).ToList();

            Console.ReadKey();
        }

    }


    [BsonIgnoreExtraElements]
    public class User
    {
        public string _id { get; set; }
        public string user_name { get; set; }
        public int age { get; set; }
        public Company company { get; set; }
    }

    public class Company
    {
        public string name { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.String)]
        public DateTime startwork { get; set; }
    }
}
