using Client.API.EventBusConsumer;
using Client.Repository.Data;
using Client.Repository.Data.Interfaces;
using Client.Repository.Repositories;
using Client.Repository.Repositories.Interfaces;
using Common;
using EventBus.Common;
using MassTransit;

namespace Client.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<IClientContext, ClientContext>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<EncryptionService>();

            // MassTransit-RabbitMQ Configuration
            builder.Services.AddMassTransit(config => {

                config.AddConsumer<ClientInfoConsumer>();

                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

                    cfg.ReceiveEndpoint(EventBusConstants.ClientInfoQueue, c => {
                        c.ConfigureConsumer<ClientInfoConsumer>(ctx);
                    });
                });
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}