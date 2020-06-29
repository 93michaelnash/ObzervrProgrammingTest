using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObzervrProgrammingTest.Services
{
    public interface IBigQueryTaxiDataService
    {
       Task<List<string>> GetAllTaxiResultsBetweenTwoDates(DateTime dateStart, DateTime dateEnd);
    }
}