using System.Text.Json.Serialization;

namespace Consumer.Response;

public class CustomerResponse(int id, string name, string status)
{
    [JsonPropertyName("id")] public int Id { get; set; } = id;
    [JsonPropertyName("name")] public string Name { get; set; } = name;
    [JsonPropertyName("status")] public string Status { get; set; } = status;
}