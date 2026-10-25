using ImportDomain.Models;

namespace Import.Engine;

public class CsvRecordParser : IRecordParser
{
    public Transaction Parse(string line)
    {
        var fields = line.Split(',');
        if (fields.Length != 4)
            throw new FormatException($"CSV invalide : 4 colonnes attendues, {fields.Length} reçues.");

        var id = fields[0].Trim();
        if (string.IsNullOrEmpty(id))
            throw new FormatException("OriginalSourceId ne peut pas être vide.");

        return new Transaction(
            id,
            FieldConverter.Date(fields[1], "yyyy-MM-dd"),
            FieldConverter.Decimal(fields[2], "Value"),
            FieldConverter.Decimal(fields[3], "Rate")
        );
    }
}
