using Newtonsoft.Json;

namespace checkout.ViewModels;

public class ItemViewModel
{
    [JsonProperty(PropertyName = "itemid")]    
    public string ItemId { get; set; }
    [JsonProperty(PropertyName = "quantity")]
    public int Quantity { get; set; }
    [JsonProperty(PropertyName = "isavailable")]    
    public bool IsAvailable { get; set; }
    [JsonProperty(PropertyName = "priceperitem")]
    public double PricePerItem { get; set; }
    [JsonProperty(PropertyName = "url")]    
    public string Url { get; set; }
    [JsonProperty(PropertyName = "name")]    
    public string Name { get; set; }
    public ItemViewModel(string itemId, int quantity, bool isAvailable, double pricePerItem, string name, string url)
    {
        ItemId = itemId;
        Quantity = quantity;
        IsAvailable = isAvailable;
        PricePerItem = pricePerItem;
        Name = name;
        Url = url;
    }
}