using Newtonsoft.Json;

namespace checkout.ViewModels;

public class Cart
{
    [JsonProperty(PropertyName = "userid")]    
    public string UserId { get; set; }
    [JsonProperty(PropertyName = "itemid")]
    public string ItemId { get; set; }

    [JsonProperty(PropertyName = "quantity")]    
    public int Quantity { get; set; }

    public Cart(string userId, string itemId, int quantity)
    {
        UserId = userId;
        ItemId = itemId;
        Quantity = quantity;
    }
}