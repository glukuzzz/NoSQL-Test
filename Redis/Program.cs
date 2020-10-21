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
            
           // db.HashSet("hashset", new [] {new HashEntry( "entry1", "4555" ) , new HashEntry("entry2", "4555") });
           // db.HashSet("hashset", "entry3", "777");
           // var keys = db.HashKeys("hashset");
           // db.HashDelete("hashset","entry1");
           // var data = db.HashGetAll("hashset");

            
           //foreach(var y in data)  Console.WriteLine(y.Name + " " + y.Value);

            //db.SetAdd(new RedisKey("set_key"), new RedisValue("value1"));
            //db.SetAdd("set_key", "value1");
            //var y = db.SetMembers("set_key");
            //var set_items = db.SetMembers(new RedisKey("set_key"));
            //var set_random_item = db.SetPop(new RedisKey("set_key"));
            //db.ListLeftPush("u", "ggg");
            //db.ListRightPush("u", "ppp");

            //var listdata = db.ListRange("list");

            //var item = db.ListRightPop("list");

            //foreach (var y in listdata)
            //{
            //    Console.WriteLine(y);
            //}
            var transaction = db.CreateTransaction();
            transaction.HashSetAsync("hashset", new[] { new HashEntry("entry1", "4555"), new HashEntry("entry2", "4555") });
            transaction.HashSetAsync("hashset", "entry3", "777");
            transaction.HashDeleteAsync("hashset", "entry1");
            transaction.Execute();
            var data = db.HashGetAll("hashset");

            foreach (var y in data) Console.WriteLine(y.Name + " " + y.Value);

            //VAR TRAN
            //   db.TRA

            Console.ReadKey();

        }
    }    
}
