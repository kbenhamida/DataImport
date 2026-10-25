using Import.Engine;
using Xunit;

namespace ImportEngine.Tests;

public class ImportServiceTests
{
    private readonly ImportService _importService = new();
    private readonly CsvRecordParser _csvParser = new();

    [Fact]
    public void Import_WithEmptyAndWhitespaceLines_IgnoresThemAndParsesRecords()
    {
        const string content = "F001,2026-10-10,5050.89,0.145\n\n   \nF003,2026-10-20,12200.34,1.250\n";
        using var reader = new StringReader(content);

        var records = _importService.Import(reader, _csvParser);

        Assert.Equal(2, records.Count);
        Assert.Equal("F001", records[0].OriginalSourceId);
        Assert.Equal(new DateOnly(2026, 10, 10), records[0].Date);
        Assert.Equal(5050.89m, records[0].Value);
        Assert.Equal(0.145m, records[0].Rate);

        Assert.Equal("F003", records[1].OriginalSourceId);
        Assert.Equal(new DateOnly(2026, 10, 20), records[1].Date);
        Assert.Equal(12200.34m, records[1].Value);
        Assert.Equal(1.250m, records[1].Rate);
    }

    [Fact]
    public void Import_NullReader_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _importService.Import(null!, _csvParser));
    }

    [Fact]
    public void Import_NullParser_ThrowsArgumentNullException()
    {
        using var reader = new StringReader("test");
        Assert.Throws<ArgumentNullException>(() => _importService.Import(reader, null!));
    }

    [Fact]
    public void Import_InvalidLine_ThrowsImportExceptionWithCorrectLineNumber()
    {
        // Line 1: valid
        // Line 2: invalid
        const string content = "F001,2026-10-10,5050.89,0.145\nincorrect";
        using var reader = new StringReader(content);

        var ex = Assert.Throws<ImportException>(() => _importService.Import(reader, _csvParser));

        Assert.Equal(2, ex.LineNumber);
        Assert.Contains("Ligne 2", ex.Message);
        Assert.NotNull(ex.InnerException);
    }
}
