using System.Globalization;

namespace Import.Engine;

internal static class FieldConverter
{
    public static DateOnly Date(string value, string format)
    {
        if (!DateOnly.TryParseExact(value.Trim(), format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date))
            throw new FormatException($"Date invalide : '{value}' (attendu : {format}).");
        return date;
    }

    public static decimal Decimal(string value, string fieldName)
    {
        if (!decimal.TryParse(value.Trim(), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var result))
            throw new FormatException($"Nombre invalide dans {fieldName} : '{value}'.");
        return result;
    }

    public static decimal ImpliedDecimal(string value, int digits, int scale, string fieldName)
    {
        if (value.Length != digits || value.Any(c => c < '0' || c > '9'))
            throw new FormatException($"Champ {fieldName} invalide : attendu {digits} chiffres, reçu '{value}'.");
        return decimal.Parse(value, NumberStyles.None, CultureInfo.InvariantCulture)
               / Pow10(scale);
    }

    private static decimal Pow10(int exponent)
    {
        decimal result = 1;
        for (int i = 0; i < exponent; i++) result *= 10;
        return result;
    }
}
