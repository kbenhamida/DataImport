using System.Text.Json;
using ImportDomain.Models;
using Xunit;

namespace ImportEngine.Tests;

public class TransactionRecordTests
{
    [Fact]
    public void Serialization_ProducesExpectedJsonContractAndIsoDateFormat()
    {
        var record = new Transaction("F001", new DateOnly(2026, 10, 10), 5050.89m, 0.145m);

        var json = JsonSerializer.Serialize(record);

        Assert.Contains("\"OriginalSourceId\":\"F001\"", json);
        Assert.Contains("\"TransactionDate\":\"2026-10-10\"", json);
        Assert.Contains("\"Value\":5050.89", json);
        Assert.Contains("\"Rate\":0.145", json);
    }
}
