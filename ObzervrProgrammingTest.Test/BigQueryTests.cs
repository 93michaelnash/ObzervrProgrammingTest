using ObzervrProgrammingTest.Services;
using System;
using Xunit;

namespace ObzervrProgrammingTest.Test
{
    public class BigQueryTests
    {
        [Fact]
        public void TestGet()
        {
            BigQueryService.Get();
            Assert.True(true);
        }
    }
}
