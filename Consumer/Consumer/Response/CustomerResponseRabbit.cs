namespace Consumer.Response;

public class CustomerResponseRabbit(int id, string status)
{
    public int Id { get; private set; } = id;
    public string Status { get; private set; } = status;
}