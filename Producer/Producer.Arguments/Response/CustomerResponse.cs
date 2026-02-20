using System.Text.Json.Serialization;

namespace Producer.Arguments.Response;

public class CustomerResponse(int id, string name, string status)
{
    [JsonPropertyName("id")] public int Id { get; private set; } = id;
    [JsonPropertyName("name")] public string Name { get; private set; } = name;
    [JsonPropertyName("status")] public string Status { get; private set; } = status;
}