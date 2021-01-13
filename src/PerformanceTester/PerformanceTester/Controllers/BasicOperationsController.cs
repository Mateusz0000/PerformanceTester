using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PerformanceTester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicOperationsController : ControllerBase
    {

        public BasicOperationsController()
        {
        }

        // GET: api/BasicOperations
        [HttpGet]
        public IActionResult EmptyResponses() => Ok();

        [HttpGet("plaintext")]
        public IActionResult Plaintext()
        {
            return new PlainTextActionResult();
        }

        private class PlainTextActionResult : IActionResult
        {
            private static readonly byte[] _helloWorldPayload = Encoding.UTF8.GetBytes("Hello, World!");

            public Task ExecuteResultAsync(ActionContext context)
            {
                var response = context.HttpContext.Response;
                response.StatusCode = StatusCodes.Status200OK;
                response.ContentType = "text/plain";
                var payloadLength = _helloWorldPayload.Length;
                response.ContentLength = payloadLength;
                return response.Body.WriteAsync(_helloWorldPayload, 0, payloadLength);
            }
        }
    }
}
