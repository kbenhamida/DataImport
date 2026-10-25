using Import.Engine;
using System.Text.Json;

if (args.Length != 2 || (args[0] != "csv" && args[0] != "fixed"))
{
    Console.Error.WriteLine("Usage : dotnet run --project src/ImportCli -- <csv|fixed> <fichier>");
    return 2;
}

try
{
    IRecordParser parser = args[0] switch
    {
        "csv" => new CsvRecordParser(),
        "fixed" => new FixedWidthRecordParser(),
        _ => throw new ArgumentOutOfRangeException(nameof(args))
    };

    using var reader = new StreamReader(args[1]);
    
    var importService = new ImportService();
    var records = importService.Import(reader, parser);

    Console.WriteLine(JsonSerializer.Serialize(records));
    return 0;
}
catch (Exception ex) when (ex is ImportException or IOException or UnauthorizedAccessException)
{
    Console.Error.WriteLine($"Erreur d'import : {ex.Message}");
    return 1;
}
