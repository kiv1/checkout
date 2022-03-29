using Newtonsoft.Json;

namespace checkout.ViewModels;

public class User
{
    [JsonProperty(PropertyName = "id")]    
    public string Id { get; set; }
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; }

    public User(string id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;

    }
}