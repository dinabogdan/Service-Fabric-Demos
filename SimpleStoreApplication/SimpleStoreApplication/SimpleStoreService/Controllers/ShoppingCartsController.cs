using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using SimpleStoreService.Domain;
using System.Threading.Tasks;

namespace SimpleStoreService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartsController : ControllerBase
    {
        const string ShoppingCart = "shoppingCart";

        private readonly IReliableStateManager mStateManager;
        private readonly ILogger<ShoppingCartsController> _logger;

        public ShoppingCartsController(IReliableStateManager stateManager, ILogger<ShoppingCartsController> logger)
        {
            this.mStateManager = stateManager;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var myDictionary = await mStateManager.GetOrAddAsync<IReliableDictionary<string, ShoppingCart>>(ShoppingCart);

            using (var tx = mStateManager.CreateTransaction())
            {
                var result = await myDictionary.TryGetValueAsync(tx, getUserIdentity());
                if (result.HasValue)
                {
                    return new JsonResult(new
                    {
                        Total = result.Value.Total,
                        Items = result.Value.GetItems()
                    });
                }

                else return new JsonResult(new ShoppingCart());
            }
        }

        [HttpPost]
        public async void Post([FromBody] ShoppingCartItem item)
        {
            await addToCart(item);
        }

        [HttpDelete("name")]
        public async Task<IActionResult> Delete(string name)
        {
            var myDictionary = await mStateManager.GetOrAddAsync<IReliableDictionary<string, ShoppingCart>>(ShoppingCart);
            using (var tx = mStateManager.CreateTransaction())
            {
                var result = await myDictionary.TryGetValueAsync(tx, getUserIdentity());
                if (result.HasValue)
                {
                    await myDictionary.SetAsync(tx, name, result.Value.RemoveItem(name));
                    await tx.CommitAsync();

                    return new OkResult();
                }
                else return new NotFoundResult();
            }
        }

        private async Task addToCart(ShoppingCartItem item)
        {
            var myDictionary = await mStateManager.GetOrAddAsync<IReliableDictionary<string, ShoppingCart>>(ShoppingCart);

            using (var tx = mStateManager.CreateTransaction())
            {
                await myDictionary.AddOrUpdateAsync(tx, getUserIdentity(), new ShoppingCart(item), (k, v) => v.AddItem(item));
                await tx.CommitAsync();
            }
        }

        private string getUserIdentity()
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null && !string.IsNullOrEmpty(HttpContext.User.Identity.Name))
                return HttpContext.User.Identity.Name;
            else
                return "anonymous";
        }
    }
}
