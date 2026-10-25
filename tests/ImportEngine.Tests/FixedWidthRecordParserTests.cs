using Import.Engine;
using ImportDomain.Models;
using Xunit;

namespace ImportEngine.Tests;

public class FixedWidthRecordParserTests
{
    private readonly FixedWidthRecordParser _parser = new();

    [Fact]
    public void ParseFirstSampleLineReturnsExpectedTransaction()
    {
        // F001, date 2026-10-10, Value 5050.89 (00505089), Rate 0.145 (00145)
        const string line = "F001202610100050508900145";
        var expected = new Transaction("F001", new DateOnly(2026, 10, 10), 5050.89m, 0.145m);

        var result = _parser.Parse(line);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseSecondSampleLineReturnsExpectedTransaction()
    {
        // F003, date 2026-10-20, Value 12200.34 (01220034), Rate 1.250 (01250)
        const string line = "F003202610200122003401250";
        var expected = new Transaction("F003", new DateOnly(2026, 10, 20), 12200.34m, 1.250m);

        var result = _parser.Parse(line);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("F00120261010005050890014")]    // 1- 24 chars
    [InlineData("F0012026101000505089001450")]  // 2- 26 chars
    public void Parse_InvalidLineLength_ThrowsFormatException(string line)
    {
        var ex = Assert.Throws<FormatException>(() => _parser.Parse(line));
        Assert.Contains("25 caractères attendus", ex.Message);
    }

    [Fact]
    public void Parse_EmptySourceId_ThrowsFormatException()
    {
        const string line = "    202610100050508900145";
        var ex = Assert.Throws<FormatException>(() => _parser.Parse(line));
        Assert.Contains("OriginalSourceId ne peut pas être vide", ex.Message);
    }

    [Fact]
    public void Parse_InvalidDate_ThrowsFormatException()
    {
        const string line = "F001202613100050508900145"; // Month 13
        var ex = Assert.Throws<FormatException>(() => _parser.Parse(line));
        Assert.Contains("Date invalide", ex.Message);
    }

    [Theory]
    [InlineData("F0012026101000505A8900145", "Value")]
    [InlineData("F00120261010005050890014X", "Rate")]
    public void Parse_NonDigitInImpliedDecimal_ThrowsFormatException(string line, string expectedField)
    {
        var ex = Assert.Throws<FormatException>(() => _parser.Parse(line));
        Assert.Contains($"Champ {expectedField} invalide", ex.Message);
    }
}
