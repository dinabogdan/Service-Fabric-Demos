using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleStoreService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStoreWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {

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
            HttpResponseMessage responseMessage = await this._httpClient.GetAsync(await ResolveAddress() + "/api/shoppingcarts");

            if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                return this.StatusCode((int)responseMessage.StatusCode);

            var cart = JsonConvert.DeserializeObject<ShoppingCart>(await responseMessage.Content.ReadAsStringAsync());

            return new JsonResult(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ShoppingCartItem item)
        {
            StringContent postContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            postContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpResponseMessage responseMessage = await _httpClient.PostAsync(await ResolveAddress() + "/api/ShoppingCarts", postContent);

            return new OkResult();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string productName)
        {
            HttpResponseMessage responseMessage = await _httpClient.DeleteAsync(await ResolveAddress() + "/api/shoppingcarts/" + productName);

            if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                return this.StatusCode((int)responseMessage.StatusCode);

            return new OkResult();
        }

        private async Task<string> ResolveAddress()
        {
            var partitionResolver = ServicePartitionResolver.GetDefault();
            var resolveResults = await partitionResolver.ResolveAsync(
                new Uri("fabric:/SimpleStoreApplication/SimpleStoreService"),
                    new ServicePartitionKey(1),
                    new System.Threading.CancellationToken());

            var endpoint = resolveResults.GetEndpoint();
            var endpointObject = JsonConvert.DeserializeObject<JObject>(endpoint.Address);
            return ((JObject)endpointObject.Property("Endpoints").Value)[""].Value<string>();
        }
    }
}
