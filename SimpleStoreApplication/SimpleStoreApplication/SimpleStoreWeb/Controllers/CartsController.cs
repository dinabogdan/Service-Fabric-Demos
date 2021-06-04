using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleStoreWeb.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CartsController : Controller
    {

        private static Regex ipRex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

        private readonly HttpClient _httpClient;
        private readonly ILogger<CartsController> _logger;

        public CartsController(HttpClient httpClient, ILogger<CartsController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
        }

        private async Task<string> ResolveAddress()
        {

        }
    }
}
