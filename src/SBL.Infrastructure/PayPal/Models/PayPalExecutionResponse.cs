using System.Text.Json.Serialization;

namespace SBL.Infrastructure.PayPal.Models;

public class PayPalExecutionResponse
{
    [JsonPropertyName("id")] 
    public string Id { get; set; }

    [JsonPropertyName("state")] 
    public string State { get; set; }

    [JsonPropertyName("transactions")] 
    public List<PayPalTransaction> Transactions { get; set; }
}