using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ObzervrProgrammingTest.Extensions;
using ObzervrProgrammingTest.Services;

namespace ObzervrProgrammingTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxiDataController : ControllerBase
    {
        private readonly ILogger<TaxiDataController> _logger;
        private readonly IBigQueryTaxiDataService _bigQueryTaxiDataService;

        public TaxiDataController(ILogger<TaxiDataController> logger, IBigQueryTaxiDataService bigQueryTaxiDataService)
        {
            _logger = logger;
            _bigQueryTaxiDataService = bigQueryTaxiDataService;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> GetAllResultsForMonthAndYear(DateTime date)
        {
            try
            {
                _logger.LogInformation($"Getting taxi data for date {date}");
                var dateStart = date.FirstDayOfMonth();
                var dateEnd = date.LastDayOfMonth();
                return await _bigQueryTaxiDataService.GetAllTaxiResultsBetweenTwoDates(dateStart, dateEnd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
