namespace MicroRabbit.Infra.Bus;

public class RabbitMqProperties
{
    public string HostName { get; set; } = null!;
    public int Port { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string ConnectionString => $"host={HostName};port={Port};username={UserName};password={Password}";
}
