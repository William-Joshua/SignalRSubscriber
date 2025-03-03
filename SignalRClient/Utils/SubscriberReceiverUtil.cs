using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClient.Utils
{
    public class SubscriberReceiverUtil : IAsyncDisposable
    {
        private readonly HubConnection _connection;
        private readonly IConfiguration _configuration;

        public SubscriberReceiverUtil(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new HubConnectionBuilder()
                .WithUrl(_configuration.GetSection("SignalRHubUrl").Value) // 从配置中读取 Hub URL
                .WithAutomaticReconnect()
                .Build();

            Console.WriteLine($"SignalRHubUrl: {_configuration.GetSection("SignalRHubUrl").Value}");

            _connection.On<string, string>("ReceiveMessage", (producerType, jsonFormatData) =>
            {
                // 在这里处理接收到的消息
                Console.WriteLine($"Received message: {producerType}, {jsonFormatData}");
                // 你可以在这里添加你的业务逻辑
            });
        }

        public async Task StartAsync()
        {
            try
            {
                await _connection.StartAsync();
                Console.WriteLine("SignalR Connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignalR connection error: {ex.Message}");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }
        }
    }

}
