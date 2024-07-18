using Microsoft.Kiota.Abstractions.Serialization;

namespace Weather.Client;

public class WeatherSerializationWriterFactory : ISerializationWriterFactory
{
    public ISerializationWriter GetSerializationWriter(string contentType)
    {
        throw new NotImplementedException();
    }

    public string ValidContentType { get; }
}