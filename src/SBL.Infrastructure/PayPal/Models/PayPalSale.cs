using System.Text.Json.Serialization;

namespace SBL.Infrastructure.PayPal.Models;

public class PayPalSale
{
    [JsonPropertyName("id")] 
    public string Id { get; set; }

    [JsonPropertyName("state")] 
    public string State { get; set; }
}