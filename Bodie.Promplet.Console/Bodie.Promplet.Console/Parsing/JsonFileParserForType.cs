using System.Text.Json;

namespace Bodie.Promplet.Console.Parsing
{
    public class JsonFileParserForType<T>
    {
        private readonly string _sampleJsonFilePath;
        public JsonFileParserForType(string sampleJsonFilePath)
        {
            _sampleJsonFilePath = sampleJsonFilePath;
        }
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public IEnumerable<T>? ParseFile()
        {
            var data = File.ReadAllText(_sampleJsonFilePath);
            return data.Any() ? JsonSerializer.Deserialize<List<T>>(data, _options) : Enumerable.Empty<T>();
        }

    }
}
