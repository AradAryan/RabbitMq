using RabbitMq;
using MassTransit;
using System.Reflection.PortableExecutable;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<RabbitConsumer>();
            x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://127.0.0.1:5672"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ReceiveEndpoint("ReadyToSend", rc =>
                {
                    rc.Durable = true;
                    rc.UseRetry(c => c.Immediate(10));
                    rc.PrefetchCount = 1;
                    //rc.UseRateLimit(100,TimeSpan.FromSeconds(value: 60));
                    rc.ConfigureConsumer<RabbitConsumer>(provider);
                    //rc.Batch<PersonModel>(c =>
                    //{
                    //    c.MessageLimit = 110;
                    //    c.ConcurrencyLimit = 10;
                    //    c.Consumer<RabbitConsumer, PersonModel>(provider);
                    //});

                });

            }));

        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();