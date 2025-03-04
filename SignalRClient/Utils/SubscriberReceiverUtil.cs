using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClient.Utils
{
    public class SubscriberReceiverUtil : IHostedService, IDisposable
    {
        private readonly HubConnection _connection;

        public SubscriberReceiverUtil(HubConnection connection ) // 修改构造函数
        {
            _connection = connection;

            _connection.On<string, string>("ReceiveMessage", (producerType, jsonFormatData) =>
            {
                // 在这里处理接收到的消息
                Console.WriteLine($"Received message: {producerType}, {jsonFormatData}");
                // 你可以在这里添加你的业务逻辑
            });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync(cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_connection != null)
            {
                await _connection.StopAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            _connection?.DisposeAsync().GetAwaiter().GetResult();
        }
    }

}
