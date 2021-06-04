using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using CalculatorService.Interfaces.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CalculatorWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {

        // GET: api/<CalculatorController>/add
        [HttpGet]
        public string Add(int a, int b)
        {
            var calculatorClient = ServiceProxy.Create<ICalculatorService>(
                new Uri("fabric:/CalculatorApplication/CalculatorService"));
            return calculatorClient.Add(a, b).Result;
        }

        // GET: api/<CalculatorController>/subtract
        [HttpGet]
        public string Subtract(int a, int b)
        {
            var calculatorClient = ServiceProxy.Create<ICalculatorService>(
                new Uri("fabric:/CalculatorApplication/CalculatorService"));
            return calculatorClient.Subtract(a, b).Result;
        }

        // GET api/<CalculatorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CalculatorController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CalculatorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CalculatorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
