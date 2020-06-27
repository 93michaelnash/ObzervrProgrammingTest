using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using ObzervrProgrammingTest.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ObzervrProgrammingTest.Services
{
    public class BigQueryService
    {
        private BigQueryClient _client;

        public BigQueryService()
        {
            _client = BigQueryClient.Create(Constants.BiqQueryClientProjectId, GoogleCredentialHelper.GetGoogleCredentials());
        }

        public void Get()
        {
            var table = _client.GetTable("bigquery-public-data", "samples", "shakespeare");
            var sql = $"SELECT corpus AS title, COUNT(word) AS unique_words FROM {table} GROUP BY title ORDER BY unique_words DESC LIMIT 10";

            var results = _client.ExecuteQuery(sql, parameters: null);

            foreach (var row in results)
            {
                Console.WriteLine($"{row["title"]}: {row["unique_words"]}");
            }
        }
    }
}
