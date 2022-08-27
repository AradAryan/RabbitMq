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
                cfg.ReceiveEndpoint("PersonQueue", rc =>
                {
                    rc.Durable = true;
                    rc.PrefetchCount = 100;
                    //rc.ConfigureConsumer<RabbitConsumer>(provider);
                    rc.Batch<PersonModel>(c =>
                    {
                        c.MessageLimit = 10;
                        c.ConcurrencyLimit = 1;
                        c.TimeLimit = TimeSpan.FromSeconds(1);
                        c.Consumer(() => new RabbitConsumer());
                    });

                });

            }));

        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();