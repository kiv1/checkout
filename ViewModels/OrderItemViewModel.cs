using Newtonsoft.Json;

namespace checkout.ViewModels;

public class OrderItemViewModel
{
    [JsonProperty(PropertyName = "itemID")]    
    public string ItemId { get; set; }
    [JsonProperty(PropertyName = "quantity")]
    public int Quantity { get; set; }
    [JsonProperty(PropertyName = "pricePerItem")]
    public double PricePerItem { get; set; }

    public OrderItemViewModel(string itemId, int quantity, double pricePerItem)
    {
        ItemId = itemId;
        Quantity = quantity;
        PricePerItem = pricePerItem;
    }
}