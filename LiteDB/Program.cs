using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDBs
{
    class Program
    {
        static void Main(string[] args)
        {

            //очень удобно, если чего нет - создаст, избавлен от лишнего, про сравнению с mongo при томже функицонале, правильно сохраняет дату и время
            var db = new LiteDB.LiteDatabase(@"MyData.db");
            var users = db.GetCollection<User>("Users");

            //users.DeleteAll();
            users.EnsureIndex(x => new User { user_name = x.user_name }, true);
            users.EnsureIndex(x => new User { _id = x._id }, true);
            users.Insert(new User
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
            var m = users.Find(x => x.company.startwork >= new DateTime(2002, 1, 1)).ToList();
            var sigleUser = users.FindOne(x => x.user_name == "Zaza");
            
            users.DropIndex("usernameusername");
            users.DeleteMany(x => x.user_name == "Zaza");
            users.DeleteAll();

            sigleUser.company.name = "MegaHard";
            users.Update(sigleUser);
            users.UpdateMany(x => new User { age = 3 }, x => x.age > 20);
            users.DeleteAll();

            Console.WriteLine();
        }
    }

    //[MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
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
        //[BsonDateTimeOptions(Representation = BsonType.String)]
        public DateTime startwork { get; set; }
    }
}
