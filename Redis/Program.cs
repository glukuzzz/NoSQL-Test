using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis
{
    class Program
    {
        static void Main(string[] args)
        {
            //docker run --name redis -p 6379:6379 -d redis

            var redis = ConnectionMultiplexer.Connect("localhost");

            var db = redis.GetDatabase();
            

            db.StringSet("testKey", "test1");
            string str = db.StringGet("testKey");
            string str1 = db.StringGet("testKey3").ToString() ?? "no data";
            Console.WriteLine(str);
            Console.WriteLine(str1);
            db.HashSet(new RedisKey("p"), new HashEntry[] { new HashEntry(new RedisValue("hhhh"), new RedisValue("44444"))});
            db.HashSet(new RedisKey("p"), new HashEntry[] { new HashEntry(new RedisValue("zzz"), new RedisValue("4555")) });


            var t = db.HashGetAll(new RedisKey("p"));
            
           foreach(var y in t)  Console.WriteLine(y.Name + " " + y.Value);
           

            db.ListLeftPush("u", "ggg");
            db.ListLeftPush("u", "ppp");

            var listdata = db.ListRange("u");

            foreach (var y in listdata)
            {
                Console.WriteLine(y);
            }


            Console.ReadKey();

        }
    }    
}
