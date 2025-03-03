using SignalRClient.Utils;

namespace SignalRClient
{
    public class Program
    {
        public  static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // 注册 SubscriberReceiverUtil 为单例
            builder.Services.AddSingleton<SubscriberReceiverUtil>();

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


            app.MapControllers();

            var signalRClient = app.Services.GetRequiredService<SubscriberReceiverUtil>();
            signalRClient.StartAsync().GetAwaiter().GetResult();
            app.Run();
        }
    }
}
