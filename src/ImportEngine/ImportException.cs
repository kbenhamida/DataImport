namespace Import.Engine;

public class ImportException : Exception
{
    public int LineNumber { get; }

    public ImportException(int lineNumber, string message, Exception innerException)
        : base($"Ligne {lineNumber} : {message}", innerException)
    {
        LineNumber = lineNumber;
    }
}
