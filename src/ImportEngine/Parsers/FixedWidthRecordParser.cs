using ImportDomain.Models;

namespace Import.Engine;

public class FixedWidthRecordParser : IRecordParser
{
    public Transaction Parse(string line)
    {

        if (line.Length != 25)
            throw new FormatException($"Longueur invalide : 25 caractères attendus, {line.Length} reçus.");

        var id = line.Substring(0, 4).Trim();
        if (id.Length == 0)
            throw new FormatException("OriginalSourceId ne peut pas être vide.");

        return new Transaction(
            id,
            FieldConverter.Date(line.Substring(4, 8), "yyyyMMdd"),
            FieldConverter.ImpliedDecimal(line.Substring(12, 8), 8, 2, "Value"),
            FieldConverter.ImpliedDecimal(line.Substring(20, 5), 5, 3, "Rate")
        );
    }
}
