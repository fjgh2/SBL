using System.Text.Json.Serialization;

namespace SBL.Infrastructure.PayPal.Models;

public class PayPalPaymentResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("state")]
    public string State { get; set; }
    
    [JsonPropertyName("links")]
    public List<PayPalLink> Links { get; set; }
}