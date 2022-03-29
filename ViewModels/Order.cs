using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace checkout.ViewModels;

public class Order
{
    [JsonProperty(PropertyName = "id")]    
    public string Id { get; set; }
    [JsonProperty(PropertyName = "userId")]
    public string UserId { get; set; }
    [JsonProperty(PropertyName = "items")]
    public List<OrderItemViewModel> OrderItems { get; set; }

    public Order(string id, string userId, List<OrderItemViewModel> orderItems)
    {
        Id = id;
        UserId = userId;
        OrderItems = orderItems;

    }
}