using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using checkout.ViewModels;
using System.Text;
using Stripe;
using Order = checkout.ViewModels.Order;

namespace checkout.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckoutController : ControllerBase
{
    private static readonly string AuthUrl = Environment.GetEnvironmentVariable("AUTH_URL") ?? "";
    private static readonly string ComplexCartUrl = Environment.GetEnvironmentVariable("COMPLEX_CART_URL") ?? "";
    private static readonly string OrderUrl = Environment.GetEnvironmentVariable("ORDER_URL") ?? "";
    private static readonly string InventoryUrl = Environment.GetEnvironmentVariable("INVENTORY_URL") ?? "";
    private static readonly string CartUrl = Environment.GetEnvironmentVariable("CART_URL") ?? "";
    private static readonly string StripeUrl = Environment.GetEnvironmentVariable("STRIPE_URL") ?? "";
    private static readonly string StripeSecret = Environment.GetEnvironmentVariable("STRIPE_SECRET") ?? "";
    private static readonly string EmailUrl = Environment.GetEnvironmentVariable("EMAIL_URL") ?? "";

    private readonly ILogger<CheckoutController> _logger;

    public CheckoutController(ILogger<CheckoutController> logger)
    {
        _logger = logger;
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> Checkout(string userId)
    {
        if (!await CheckUser(userId))
        {
            return Unauthorized();
        }
        using var client = new HttpClient();
        Console.WriteLine(ComplexCartUrl);
        var result = await client.GetAsync($"{ComplexCartUrl}/cart/{userId}");
        var responseContent = await result.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent);
        var cartItems = JsonConvert.DeserializeObject<IEnumerable<CartViewModel>>(responseContent);
        if (cartItems.Count() == 0)
        {
            return NoContent();
        }

        bool isThereChange = false;
        var listOfOrderItemViewModel = new List<OrderItemViewModel>();
                
        var total = 0.0;
        //Check Cart
        foreach (var cart in cartItems)
        {
            if (cart.IsRemoved)
            {
                return BadRequest("There are changes to your cart as some item is not available");
            }
            total += cart.Quantity + cart.PricePerItem;
            listOfOrderItemViewModel.Add(new OrderItemViewModel(cart.ItemId, cart.Quantity, cart.PricePerItem));
        }

        //Create order obj
        var order = new Order(Guid.NewGuid().ToString(), userId, listOfOrderItemViewModel);

        //Create payment intent
        var svm = new StripeViewModel(order.Id, total, userId); 
        var json = JsonConvert.SerializeObject(svm);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        result = await client.PostAsync($"{StripeUrl}/stripe/create-payment-intent",data);
        var contents = await result.Content.ReadAsStringAsync();

        // Create Order in db
        json = JsonConvert.SerializeObject(order);
        data = new StringContent(json, Encoding.UTF8, "application/json");
        await client.PostAsync($"{OrderUrl}/order/create",data);
        
        //Delete item from cart
        foreach (var cart in cartItems)
        {
            await client.DeleteAsync($"{CartUrl}/cart/{userId}/{cart.ItemId}");
        }

        return Ok(contents);
    }

    [HttpPost]
    public async Task<IActionResult> Webhook()
    {
        string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], StripeSecret);
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    //make sure payment info is valid
                    var intent = (PaymentIntent)stripeEvent.Data.Object;
                    var orderId = intent.Metadata["orderId"];
                    var clientId = intent.Metadata["clientId"];
                    
                    var client = new HttpClient();
                    var data = new StringContent("{\"orderStatus\":\"Pending\"}", Encoding.UTF8, "application/json");
                    await client.PutAsync($"{OrderUrl}/order/{orderId}",data);

                    var order_result = client.GetAsync($"{OrderUrl}/order/orderid/{orderId}");
                    var responseContent = await order_result.Result.Content.ReadAsStringAsync();
                    var orderedItems = JsonConvert.DeserializeObject<FullOrderDetails>(responseContent);
                    foreach (var oneItem in orderedItems.Details)
                    {
                        data = new StringContent("{\"quantity\":\""+oneItem.Quantity+"\"}", Encoding.UTF8, "application/json");
                        await client.PutAsync($"{InventoryUrl}/inventory/update-value/{oneItem.ItemId}",data);

                    }
                    var userResult = client.GetAsync($"{AuthUrl}/auth/user/{clientId}");
                    responseContent = await userResult.Result.Content.ReadAsStringAsync();
                    
                    var user = JsonConvert.DeserializeObject<User>(responseContent);
                    data = new StringContent("{\"name\":\""+user.Name+"+\",\"email\":\""+user.Email+"\"}", Encoding.UTF8, "application/json");
                    await client.PostAsync($"{EmailUrl}/email/order/{orderId}", data);
                    
                    break;
            }
            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
        return Ok();
    }

    private async Task<bool> CheckUser(string userId)
    {
        using var client = new HttpClient();
        var result = await client.GetAsync($"{AuthUrl}/auth/check/{userId}");
        var responseContent = await result.Content.ReadAsStringAsync();
        if (int.TryParse(responseContent, out int val))
        {
            if (val == 1)
            {
                return true;
            }

            return false;
        }

        return false;
        
    }
  

}