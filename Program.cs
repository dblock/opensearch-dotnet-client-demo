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
        protected class Person
        {
            public int Id { get; set; }
            public string? FirstName { get; set; }
        };

        static void Main(string[] args)
        {
            var endpoint = new Uri(Environment.GetEnvironmentVariable("OPENSEARCH_ENDPOINT") ?? throw new ArgumentNullException("Missing OPENSEARCH_ENDPOINT."));
            var region = Amazon.RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("OPENSEARCH_REGION") ?? "us-west-2");
            var connection = new AwsSigV4HttpConnection(region);
            var config = new ConnectionSettings(endpoint, connection);
            var client = new OpenSearchClient(config);

            Console.WriteLine($"{client.RootNodeInfo().Version.Distribution}: {client.RootNodeInfo().Version.Number}");

            var index_name = "sample-index";
            var index = client.Indices.Create(index_name);

            try
            {
                // index a document
                var document = new Person()
                {
                    Id = 1,
                    FirstName = "Bruce"
                };

                client.Index(document, idx => idx.Index(index_name));

                // wait for the document to index
                Thread.Sleep(3 * 1000);

                var results = client.Search<Person>(s => s
                    .Index(index_name)
                    .Query(q => q.Match(
                        mq => mq.Field(f => f.FirstName)
                            .Query("bruce")))
                    );

                foreach (var result in results.Hits)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(
                        result, 
                        Formatting.Indented
                    ));
                }

                client.Delete<Person>(document, idx => idx.Index(index_name));
            }
            finally
            {
                client.Indices.Delete(index_name);
            }
        }
    }
}