using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PerformanceTester.Data;

namespace PerformanceTester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultipleQueriesController : ControllerBase
    {
        [HttpGet("ef")]
        [Produces("application/json")]
        public Task<World[]> Ef(int queries = 1)
        {
            return ExecuteQuery<EfDb>(queries);
        }

        private Task<World[]> ExecuteQuery<T>(int queries) where T : IDb
        {
            queries = queries < 1 ? 1 : queries > 500 ? 500 : queries;
            var db = HttpContext.RequestServices.GetRequiredService<T>();
            return db.LoadMultipleQueriesRows(queries);
        }
    }
}