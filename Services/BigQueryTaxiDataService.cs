using Google.Cloud.BigQuery.V2;
using Google.Cloud.Storage.V1;
using ObzervrProgrammingTest.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ObzervrProgrammingTest.Services
{
    public class BigQueryTaxiDataService : IBigQueryTaxiDataService
    {
        private const string _publicProject = "bigquery-public-data";
        private const string _dataSet = "new_york_taxi_trips";
        private const string _dataTable = "tlc_yellow_trips_2015";

        private BigQueryClient _client;

        public BigQueryTaxiDataService()
        {
            _client = BigQueryClient.Create(Constants.BiqQueryClientProjectId, GoogleCredentialHelper.GetGoogleCredentials());
        }

        public async Task<List<string>> GetAllTaxiResultsBetweenTwoDates(DateTime dateStart, DateTime dateEnd)
        {
            var publicTable = _client.GetTable(_publicProject, _dataSet, _dataTable);

            var sql = @$"
                        SELECT TO_JSON_STRING(t)
                        FROM (
                            SELECT 
	                            dropoff_longitude, dropoff_latitude,
	                            pickup_longitude, pickup_latitude
                            FROM {publicTable}
                            WHERE 
                            pickup_datetime BETWEEN @dateStart AND @dateEnd 
                            AND dropoff_datetime BETWEEN @dateStart AND @dateEnd
                            AND ((pickup_latitude BETWEEN -90 AND 90) AND (pickup_longitude BETWEEN -180 AND 180)) 
                            AND ((dropoff_latitude BETWEEN -90 AND 90) AND (dropoff_longitude BETWEEN -180 AND 180))
                            LIMIT 10000
                         ) AS t";

            BigQueryParameter[] parameters = new[]
                {
                    new BigQueryParameter("dateStart", BigQueryDbType.DateTime, dateStart.ToString("yyyy-MM-dd")),
                    new BigQueryParameter("dateEnd", BigQueryDbType.DateTime, dateEnd.ToString("yyyy-MM-dd"))
                };

            var results = new List<string>();

            var bigQueryResult = await _client.ExecuteQueryAsync(sql, parameters: parameters);
            var rows = bigQueryResult.GetRowsAsync();
            await foreach (var row in rows)
            {
                results.Add(row[0] as string);
            }

            return results;
        }
    }
}
