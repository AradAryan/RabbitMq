using RabbitMq;
using MassTransit;
using System.Reflection.PortableExecutable;


public class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            //x.AddConsumer<RabbitConsumer>();
            x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://127.0.0.1:5672"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });



                //cfg.ReceiveEndpoint("ReadyToSend_Batch", rc =>
                //{
                //    rc.Durable = true;
                //    //rc.UseRetry(c => c.Immediate(1));
                //    rc.PrefetchCount = 1500;
                //    //rc.UseRateLimit(10000, TimeSpan.FromMilliseconds(10000000));
                //    //rc.ConfigureConsumer<RabbitConsumer>(provider);
                //    rc.Batch<BatchModel>(c =>
                //    {
                //        c.MessageLimit = 1;
                //        c.TimeLimit = TimeSpan.FromMilliseconds(500);
                //        c.ConcurrencyLimit = 1;
                //        c.Consumer<RabbitConsumer, BatchModel>(provider);
                //    });

                //});

            }));

        });
        services.AddHostedService<Worker>();
    })
    .Build();
        await host.RunAsync();

        Console.WriteLine("RunAsync Done");
    }
}