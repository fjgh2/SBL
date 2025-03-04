using System.Text.Json.Serialization;

namespace SBL.Infrastructure.PayPal.Models;

public class PayPalLink
{
    [JsonPropertyName("href")]
    public string Href { get; set; }
    
    [JsonPropertyName("rel")]
    public string Rel { get; set; }
    
    [JsonPropertyName("method")]
    public string Method { get; set; }
}