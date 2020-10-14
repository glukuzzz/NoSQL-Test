using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Neo4jClient.Cypher;

namespace Neo4j
{
    class Program
    {
        static  void Main(string[] args)
        {
            //https://github.com/neo4j-examples/movies-dotnet-neo4jclient
            //docker run  --name testneo4j  -p7474:7474 -p7687:7687  -d -v c:/neo4j/data:/data -v c:/neo4j/logs:/logs  -v c:/neo4j/import:/var/lib/neo4j/import  -v c:/neo4j/plugins:/plugins --env NEO4J_AUTH=neo4j/test  neo4j:latest
            var url = "http://localhost:7474";
            var user = "neo4j";
            var password = "test";
            var client = new GraphClient(new Uri(url), user, password);
            client.ConnectAsync().Wait();
           var yy = client.IsConnected;



            var query = client.Cypher
               .Match("(m:Movie)<-[:ACTED_IN]-(a:Person)")
               .Return((m, a) => new
               {
                   movie = m.As<Movie>().title,
                   cast = Return.As<string>("collect(a.name)")
               })
               .Limit(100);

            //You can see the cypher query here when debugging
            var data = query.ResultsAsync.Result.ToList();

            var nodes = new List<NodeResult>();
            var rels = new List<object>();
            int i = 0, target;
            foreach (var item in data)
            {
                nodes.Add(new NodeResult { title = item.movie, label = "movie" });
                target = i;
                i++;
                if (!string.IsNullOrEmpty(item.cast))
                {
                    var casts = JsonConvert.DeserializeObject<JArray>(item.cast);
                    foreach (var cast in casts)
                    {
                        var source = nodes.FindIndex(c => c.title == cast.Value<string>());
                        if (source == -1)
                        {
                            nodes.Add(new NodeResult { title = cast.Value<string>(), label = "actor" });
                            source = i;
                            i += 1;
                        }
                        rels.Add(new { source = source, target = target });
                    }
                }
            }

            var result = (new { nodes = nodes, links = rels });
            Console.ReadKey();
        }
    }

    public class NodeResult
    {
        public string title { get; set; }
        public string label { get; set; }
    }

    public class Movie
    {
        public string title { get; set; }
        public int released { get; set; }
        public string tagline { get; set; }
    }
}
