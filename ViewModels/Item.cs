using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace checkout.ViewModels;

public class Item
{
    [JsonProperty(PropertyName = "itemId")]    
    public string ItemId { get; set; }
    [JsonProperty(PropertyName = "quantity")]
    public int Quantity { get; set; }

    public Item(string itemId, int quantity)
    {
        ItemId = itemId;
        Quantity = quantity;
    }
}