using Newtonsoft.Json;

namespace checkout.ViewModels;

public class CartViewModel
{
    [JsonProperty(PropertyName = "itemId")]
    public string ItemId { get; set; }
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
    
    [JsonProperty(PropertyName = "url")]
    public string Url { get; set; }

    [JsonProperty(PropertyName = "quantity")]    
    public int Quantity { get; set; }
    [JsonProperty(PropertyName = "priceperitem")]
    public double PricePerItem { get; set; }
    [JsonProperty(PropertyName = "isremoved")]
    public bool IsRemoved { get; set; }
    public CartViewModel(string name, string itemId, double pricePerItem, int quantity, string url, bool isRemoved)
    {
        ItemId = itemId;
        Quantity = quantity;
        Name = name;
        Url = url;
        PricePerItem = pricePerItem;
        IsRemoved = isRemoved;
    }
}