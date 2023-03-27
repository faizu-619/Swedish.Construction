using EventBus.Common;
using MassTransit;
using Contractor.API.EventBusConsumer;

namespace Contractor.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // MassTransit-RabbitMQ Configuration
            builder.Services.AddMassTransit(config => {

                config.AddConsumer<ContractorArrangeConsumer>();

                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

                    cfg.ReceiveEndpoint(EventBusConstants.ContractorArrangeQueue, c => {
                        c.ConfigureConsumer<ContractorArrangeConsumer>(ctx);
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