using System.Text.Json.Serialization;

namespace SBL.Infrastructure.PayPal.Models;

public class PayPalTransaction
{
    [JsonPropertyName("related_resources")]
    public List<PayPalRelatedResource> RelatedResources { get; set; }
}