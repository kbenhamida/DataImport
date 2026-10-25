using System.Text.Json.Serialization;

namespace ImportDomain.Models
{
    public record Transaction(
    [property: JsonPropertyName("OriginalSourceId")] 
    string OriginalSourceId,
    [property: JsonPropertyName("TransactionDate")] 
    DateOnly Date,
    [property: JsonPropertyName("Value")] 
    decimal Value,
    [property: JsonPropertyName("Rate")] 
    decimal Rate);
}
