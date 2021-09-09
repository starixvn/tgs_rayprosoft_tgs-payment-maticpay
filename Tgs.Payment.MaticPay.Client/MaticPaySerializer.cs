using System.Text.Json;

namespace Tgs.Payment.MaticPay
{
    public class MaticPaySerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public string Serialize<T>(T data)
        {
            return JsonSerializer.Serialize(data, _jsonSerializerOptions);
        }

        public T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
        }
    }
}