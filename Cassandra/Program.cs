using Cassandra;
using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace CassTest
{
    class Program
    {
        //  https://github.com/Azure-Samples/azure-cosmos-db-cassandra-dotnet-getting-started/tree/master/CassandraQuickStartSample
        // docker run --name cassandra -p 9042:9042  -d cassandra
        // Cassandra Cluster Configs      
        //private const string UserName = "<FILLME>";
       // private const string Password = "<FILLME>";
       // private const string CassandraContactPoint = "127.0.0.1";  // DnsName  
        //private static int CassandraPort = 9042;

        public static void Main(string[] args)
        {
            // Connect to cassandra cluster  (Cassandra API on Azure Cosmos DB supports only TLSv1.2)
           // var options = new SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);
          //  options.SetHostNameResolver((ipAddress) => CassandraContactPoint);
            Cluster cluster = Cluster.Builder().AddContactPoint("localhost").Build();
            ISession session = cluster.Connect();

            // Creating KeySpace and table
            session.Execute("DROP KEYSPACE IF EXISTS uprofile");
            session.Execute("CREATE KEYSPACE uprofile WITH REPLICATION = { 'class' : 'NetworkTopologyStrategy', 'datacenter1' : 1 };");
            Console.WriteLine(String.Format("created keyspace uprofile"));
            session.Execute("CREATE TABLE IF NOT EXISTS uprofile.user (user_id int PRIMARY KEY, user_name text, user_bcity text)");
            Console.WriteLine(String.Format("created table user"));

            session = cluster.Connect("uprofile");
            IMapper mapper = new Mapper(session);

            // Inserting Data into user table
            mapper.Insert<User>(new User(1, "LyubovK", "Dubai"));
            mapper.Insert<User>(new User(2, "JiriK", "Toronto"));
            mapper.Insert<User>(new User(3, "IvanH", "Mumbai"));
            mapper.Insert<User>(new User(4, "LiliyaB", "Seattle"));
            mapper.Insert<User>(new User(5, "JindrichH", "Buenos Aires"));
            Console.WriteLine("Inserted data into user table");

            Console.WriteLine("Select ALL");
            Console.WriteLine("-------------------------------");
            foreach (User u in mapper.Fetch<User>("Select * from user"))
            {
                Console.WriteLine(u);
            }
            IEnumerable<User> users = mapper.Fetch<User>("Select * from user");
            var user = mapper.FirstOrDefault<User>("Select * from user where user_id = ?", 3);

            Console.WriteLine("Getting by id 3");
            Console.WriteLine("-------------------------------");
            User userId3 = mapper.FirstOrDefault<User>("Select * from user where user_id = ?", 3);
            Console.WriteLine(userId3);

            // Clean up of Table and KeySpace
            session.Execute("DROP table user");
            session.Execute("DROP KEYSPACE uprofile");

            // Wait for enter key before exiting  
            Console.ReadLine();
        }

        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
    }

    public partial class User
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_bcity { get; set; } 
    }
    public partial class User
    {
        public User(int user_id, String user_name, String user_bcity)
        {
            this.user_id = user_id;
            this.user_name = user_name;
            this.user_bcity = user_bcity;
        }

        public override String ToString()
        {
            return String.Format(" {0} | {1} | {2} ", user_id, user_name, user_bcity);
        }
    }
}
