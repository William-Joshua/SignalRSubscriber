using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using SignalRClient.Utils;

namespace SignalRClient
{
    public class Program
    {
        public  static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 配置跨域
            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()  // allow credentials
                ));


            // 获取 appsettings.json 中的 SignalRHubUrl 配置
            var signalRHubUrl = builder.Configuration["SignalRHubUrl"];

            // 注册 HubConnection 工厂
            builder.Services.AddSingleton(provider =>
            {
                return new HubConnectionBuilder()
                    .WithUrl(signalRHubUrl)
                    .Build();
            });

            builder.Services.AddHostedService<SubscriberReceiverUtil>(provider =>
            {
                var hubConnection = provider.GetRequiredService<HubConnection>();
                Task.Run(async () =>
                {
                    try
                    {
                        await hubConnection.StartAsync();
                        Console.WriteLine("SignalR connected");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"SignalR connection error: {ex.Message}");
                    }
                }).GetAwaiter().GetResult();

                return new SubscriberReceiverUtil(hubConnection);
            });
            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            app.MapControllers();

            app.Run();
        }
    }
}
