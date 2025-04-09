
using Moneybase.API.Interfaces;
using Moneybase.API.Interfaces.Impl;
using MoneyBase.API.Interfaces;
using MoneyBase.API.Interfaces.Impl;
using MoneyBase.Core.Services;
using MoneyBase.Core.Services.Impl;

namespace Moneybase.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = builder.Configuration;

            // Add services to the container.
            builder.Services.AddSingleton<IRabbitMQService>(new RabbitMQService(config["RabbitMQ:hostname"]));
            builder.Services.AddSingleton<IChatService, ChatService>();
            builder.Services.AddSingleton<IMessageProducer, RabbitMQProducer>();


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


            app.MapControllers();

            app.Run();
        }
    }
}
