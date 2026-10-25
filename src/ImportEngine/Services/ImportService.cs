using ImportDomain.Models;

namespace Import.Engine;

public class ImportService
{
    /// <summary>
    /// Imports transactions from a text reader using the specified record parser.
    /// </summary>
    /// <param name="reader">The text reader to read lines from.</param>
    /// <param name="parser">The record parser to parse each line.</param>
    /// <returns>A read-only list of transactions.</returns>
    /// <exception cref="ImportException">Thrown when a line cannot be parsed.</exception>
    public IReadOnlyList<Transaction> Import(TextReader reader, IRecordParser parser)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(parser);

        var result = new List<Transaction>();
        string? line;
        int lineNumber = 0;
        while ((line = reader.ReadLine()) is not null)
        {
            lineNumber++;
            if (string.IsNullOrWhiteSpace(line)) continue;
            try
            {
                result.Add(parser.Parse(line));
            }
            catch (FormatException ex)
            {
                throw new ImportException(lineNumber, ex.Message, ex);
            }
        }
        return result;
    }
}
