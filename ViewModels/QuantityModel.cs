using Newtonsoft.Json;

namespace checkout.ViewModels;

public class QuantityModel
{
    [JsonProperty(PropertyName = "quantity")]
    public int Quantity {get; set;}
    public QuantityModel(int quantity)
    {
        Quantity = quantity;
    }
}