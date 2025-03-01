using System.Text.Json.Serialization;

namespace SBL.Infrastructure.PayPal.Models;

public class PayPalRelatedResource
{
    [JsonPropertyName("sale")] 
    public PayPalSale Sale { get; set; }
}