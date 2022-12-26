/* SPDX-License-Identifier: Apache-2.0
*
* The OpenSearch Contributors require contributions made to
* this file be licensed under the Apache-2.0 license or a
* compatible open source license.
*/

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenSearch.Client;
using OpenSearch.Net.Auth.AwsSigV4;

namespace Application
{
    class Program
    {
        protected class Movie
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Director { get; set; }
            public int? Year { get; set; }
        };

        static void Main(string[] args)
        {
            var endpoint = new Uri(Environment.GetEnvironmentVariable("OPENSEARCH_ENDPOINT") ?? throw new ArgumentNullException("Missing OPENSEARCH_ENDPOINT."));
            var connection = new AwsSigV4HttpConnection();
            var config = new ConnectionSettings(endpoint, connection);
            var client = new OpenSearchClient(config);

            Console.WriteLine($"{client.RootNodeInfo().Version.Distribution}: {client.RootNodeInfo().Version.Number}");

            var index_name = "movies";
            var index = client.Indices.Create(index_name);

            try
            {
                // index a document
                var document = new Movie()
                {
                    Id = 1,
                    Title = "Moneyball",
                    Director = "Bennett Miller",
                    Year = 2011
                };

                client.Index(document, idx => idx.Index(index_name));

                // wait for the document to index
                Thread.Sleep(1 * 1000);

                var results = client.Search<Movie>(s => s
                    .Index(index_name)
                    .Query(q => q.Match(
                        mq => mq.Field(f => f.Director)
                            .Query("miller")))
                    );

                foreach (var result in results.Hits)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(
                        result.Source,
                        Formatting.Indented
                    ));
                }

                client.Delete<Movie>(document, idx => idx.Index(index_name));
            }
            finally
            {
                client.Indices.Delete(index_name);
            }
        }
    }
}