using Google.Cloud.BigQuery.V2;
using Google.Cloud.Storage.V1;
using ObzervrProgrammingTest.Helpers;
using System;
using System.IO;

namespace ObzervrProgrammingTest.Services
{
    public class BigQueryTaxiDataService
    {
        private const string _publicProject = "bigquery-public-data";
        private const string _dataSet = "new_york_taxi_trips";
        private const string _dataTable = "tlc_yellow_trips_2015";

        private const string _obzervrProject = "obzervr-taxi";
        private const string _obzervrDataset = "new_york_city_taxi";

        private const string _obzervrGcBucket = "obzervr-taxi-bucket";

        private BigQueryClient _client;

        public BigQueryTaxiDataService()
        {
            _client = BigQueryClient.Create(Constants.BiqQueryClientProjectId, GoogleCredentialHelper.GetGoogleCredentials());
        }

        public string CreateTaxiDataTable(DateTime dateStart, DateTime dateEnd)
        {
            var monthRange = dateStart.Month == dateStart.Month ? $"{dateStart.Month}" : $"{dateStart.Month}-{dateEnd.Month}";
            var year = $"{dateStart.Year}";
            var tableName = $"{monthRange}_{year}";

            var publicTable = _client.GetTable(_publicProject, _dataSet, _dataTable);
            var projectTable = $"obzervr-taxi.new_york_city_taxi.{tableName}";

            var sql = @$"CREATE TABLE IF NOT EXISTS {projectTable}
                        OPTIONS(
                          expiration_timestamp=TIMESTAMP_ADD(CURRENT_TIMESTAMP(), INTERVAL 1 HOUR)
                        )
                        AS
                        SELECT 
	                        dropoff_longitude, dropoff_latitude,
	                        pickup_longitude, pickup_latitude
                        FROM {publicTable}
                        WHERE 
                        pickup_datetime BETWEEN @dateStart AND @dateEnd 
                        AND dropoff_datetime BETWEEN @dateStart AND @dateEnd
                        AND ((pickup_latitude BETWEEN -90 AND 90) AND (pickup_longitude BETWEEN -180 AND 180)) 
                        AND ((dropoff_latitude BETWEEN -90 AND 90) AND (dropoff_longitude BETWEEN -180 AND 180))";

            BigQueryParameter[] parameters = new[]
                {
                    new BigQueryParameter("dateStart", BigQueryDbType.DateTime, dateStart.ToString("yyyy-MM-dd")),
                    new BigQueryParameter("dateEnd", BigQueryDbType.DateTime, dateEnd.ToString("yyyy-MM-dd"))
                };

            _client.ExecuteQuery(sql, parameters: parameters);
            return tableName;
        }

        public string GetCsvFileUrlFromObservrDataTable(string tableName) 
        {
            var table = _client.GetTable(_obzervrProject, _obzervrDataset, tableName);
            var fileName = $"{tableName}.csv";
            string destinationUri = $"gs://{_obzervrGcBucket}/{fileName}";
            string accessibleUri = $"https://storage.googleapis.com/{_obzervrGcBucket}/{tableName}";

            if (!GoogleStorageHelper.FileExists(_obzervrGcBucket, fileName))
            {
                BigQueryJob job = table.CreateExtractJob(
                  destinationUri: destinationUri
                );
                job = job.PollUntilCompleted().ThrowOnAnyError();  // Waits for the job to complete.
            }

            return accessibleUri;
        }

        private string DownloadCsv(string bucketName, string objectName, string localPath = null)
        {
            var storage = StorageClient.Create();
            localPath = localPath ?? Path.GetFileName(objectName);
            using (var outputFile = File.OpenWrite(localPath))
            {
                storage.DownloadObject(bucketName, objectName, outputFile);
            }
            return localPath;
        }
    }
}
