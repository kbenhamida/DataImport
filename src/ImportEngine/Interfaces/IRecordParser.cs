using ImportDomain.Models;

namespace Import.Engine;

public interface IRecordParser
{
    Transaction Parse(string line);
}
