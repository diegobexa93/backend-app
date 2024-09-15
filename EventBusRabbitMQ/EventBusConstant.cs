namespace EventBusRabbitMQ;

public static class RabbitMQQueues
{
    public const string UserLogAPI = "userLogAPIQueue";
    public const string UserTraceAPI = "userTraceAPIQueue";

}

public class RabbitMQSetting
{
    public string? HostName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public int Port { get; set; }
}

