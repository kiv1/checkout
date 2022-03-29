using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace checkout.ViewModels;

public class AmountViewModel
{
    [JsonProperty(PropertyName = "amount")]
    public int Amuount { get; set; }

    public AmountViewModel(int amount)
    {
        Amuount = amount;
    }
}