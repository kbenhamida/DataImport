using Import.Engine;
using ImportDomain.Models;
using Xunit;

namespace ImportEngine.Tests;

public class CsvRecordParserTests
{
    private readonly CsvRecordParser _parser = new();

    [Fact]
    public void Parse_ValidLine_ReturnsExpectedTransaction()
    {
        const string line = "F001,2026-10-10,5050.89,0.145";
        var expected = new Transaction("F001", new DateOnly(2026, 10, 10), 5050.89m, 0.145m);

        var result = _parser.Parse(line);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("F001,2026-10-10,5050.89")]
    [InlineData("F001,2026-10-10,5050.89,0.145,extra")]
    public void Parse_InvalidColumnCount_ThrowsFormatException(string line)
    {
        var ex = Assert.Throws<FormatException>(() => _parser.Parse(line));
        Assert.Contains("4 colonnes attendues", ex.Message);
    }

    [Theory]
    [InlineData("   ,2026-10-10,5050.89,0.145")]
    [InlineData(",2026-10-10,5050.89,0.145")]
    public void Parse_EmptySourceId_ThrowsFormatException(string line)
    {
        var ex = Assert.Throws<FormatException>(() => _parser.Parse(line));
        Assert.Contains("OriginalSourceId ne peut pas être vide", ex.Message);
    }

    [Fact]
    public void Parse_InvalidDate_ThrowsFormatException()
    {
        const string line = "F001,2026/10/10,5050.89,0.145";
        var ex = Assert.Throws<FormatException>(() => _parser.Parse(line));
        Assert.Contains("Date invalide", ex.Message);
    }

    [Theory]
    [InlineData("F001,2026-10-10,abc,0.145", "Value")]
    [InlineData("F001,2026-10-10,5050.89,xyz", "Rate")]
    public void Parse_InvalidDecimal_ThrowsFormatException(string line, string expectedField)
    {
        var ex = Assert.Throws<FormatException>(() => _parser.Parse(line));
        Assert.Contains($"Nombre invalide dans {expectedField}", ex.Message);
    }
}
