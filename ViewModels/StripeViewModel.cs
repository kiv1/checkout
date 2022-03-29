using Newtonsoft.Json;

namespace checkout.ViewModels;

public class StripeViewModel
{
    [JsonProperty(PropertyName = "orderId")]    
    public string OrderId { get; set; }
    [JsonProperty(PropertyName = "clientId")]    
    public string ClientId { get; set; }
    [JsonProperty(PropertyName = "amount")]
    public double Amount { get; set; }

    public StripeViewModel(string orderId, double amount, string clientId)
    {
        OrderId = orderId;
        Amount = amount;
        ClientId = clientId;
    }
}