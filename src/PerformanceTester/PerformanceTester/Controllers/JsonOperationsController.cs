using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PerformanceTester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonOperationsController : ControllerBase
    {
        private static readonly DateTimeOffset BaseDateTime = new DateTimeOffset(new DateTime(2019, 04, 23));

        private static readonly List<Entry> _entries4k = Enumerable.Range(1, 8).Select(i => new Entry
        {
            Attributes = new Attributes
            {
                Created = BaseDateTime.AddDays(i),
                Enabled = true,
                Expires = BaseDateTime.AddDays(i).AddYears(1),
                NotBefore = BaseDateTime,
                RecoveryLevel = "Purgeable",
                Updated = BaseDateTime.AddSeconds(i),
            },
            ContentType = "application/xml",
            Id = "https://benchmarktest.id/item/value" + i,
            Tags = new[] { "test", "perf", "json" },
        }).ToList();

        private static readonly List<Entry> _entries1MB = Enumerable.Range(1, 2625).Select(i => new Entry
        {
            Attributes = new Attributes
            {
                Created = BaseDateTime.AddDays(i),
                Enabled = true,
                Expires = BaseDateTime.AddDays(i).AddYears(1),
                NotBefore = BaseDateTime,
                RecoveryLevel = "Purgeable",
                Updated = BaseDateTime.AddSeconds(i),
            },
            ContentType = "application/xml",
            Id = "https://benchmarktest.id/item/value" + i,
            Tags = new[] { "test", "perf", "json" },
        }).ToList();

        [HttpGet("json")]
        [Produces("application/json")]
        public object Json()
        {
            return new { message = "Hello, World!" };
        }

        // Note that this produces 4kb data. We're leaving the misnamed scenario as is to avoid loosing historical context
        [HttpGet("json4k")]
        [Produces("application/json")]
        public object Json4k() => _entries4k;

        [HttpGet("json1M")]
        [Produces("application/json")]
        public List<Entry> Json1M() => _entries1MB;

        [HttpPost("jsoninput")]
        [Consumes("application/json")]
        public ActionResult JsonInput([FromBody] List<Entry> entry) => Ok();
    }

    public partial class Entry
    {
        public Attributes Attributes { get; set; }
        public string ContentType { get; set; }
        public string Id { get; set; }
        public bool Managed { get; set; }
        public string[] Tags { get; set; }
    }

    public partial class Attributes
    {
        public DateTimeOffset Created { get; set; }
        public bool Enabled { get; set; }
        public DateTimeOffset Expires { get; set; }
        public DateTimeOffset NotBefore { get; set; }
        public string RecoveryLevel { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}