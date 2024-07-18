using Microsoft.Kiota.Abstractions.Serialization;

namespace Weather.Client;

public class WeatherParseNodeFactory : IParseNodeFactory
{
    public IParseNode GetRootParseNode(string contentType, Stream content)
    {
        throw new NotImplementedException();
    }

    public string ValidContentType { get; }
}