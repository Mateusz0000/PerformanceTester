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
    public class SingleQueryController : ControllerBase
    {
        [HttpGet("ef")]
        [Produces("application/json")]
        public Task<World> Ef()
        {
            return ExecuteQuery<EfDb>();
        }

        private Task<World> ExecuteQuery<T>() where T : IDb
        {
            var db = HttpContext.RequestServices.GetRequiredService<T>();
            return db.LoadSingleQueryRow();
        }
    }
}