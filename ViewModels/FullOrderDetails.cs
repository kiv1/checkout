using Newtonsoft.Json;

namespace checkout.ViewModels;

public class FullOrderDetails
{
    [JsonProperty(PropertyName = "orderid")]    
    public string OrderId { get; set; }
    [JsonProperty(PropertyName = "user_id")]
    public string UserId { get; set; }
    [JsonProperty(PropertyName = "orderstatus")]
    public string OrderStatus { get; set; }
    [JsonProperty(PropertyName = "details")]
    public List<OrderDetails> Details { get; set; }

    public FullOrderDetails(string orderId, string userId, string orderStatus, List<OrderDetails> details)
    {
        OrderId = orderId;
        UserId = userId;
        OrderStatus = orderStatus;
        Details = details;
    }
}

public class OrderDetails
{
    [JsonProperty(PropertyName = "orderid")]    
    public string OrderId { get; set; }
    [JsonProperty(PropertyName = "itemid")]
    public string ItemId { get; set; }
    [JsonProperty(PropertyName = "quantity")]
    public int Quantity { get; set; }

    public OrderDetails(string orderId, string itemId, int quantity)
    {
        OrderId = orderId;
        ItemId = itemId;
        Quantity = quantity;
    }
}