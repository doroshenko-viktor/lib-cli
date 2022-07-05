using Recycle.CLI.Interfaces;

namespace Recycle.CLI.Services;

public class Service : IService
{
    public string GetString()
    {
        return "from service";
    }
}