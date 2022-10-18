using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Channels;

namespace RabbitClient
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> Logger;
        private ConnectionFactory Factory { get; }
        private ServiceSettingsConfig_Model RabbitConfigs { get; set; } = new();

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            Logger = logger;

            configuration.Bind("ServiceSetting", RabbitConfigs);

            Factory = new ConnectionFactory() { Port = RabbitConfigs.RabbitMqPort, HostName = RabbitConfigs.RabbitMqHost, UserName = RabbitConfigs.RabbitMqUser, Password = RabbitConfigs.RabbitMqPass };

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            ConfigureConsumer();
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    await Task.Delay(1000, stoppingToken);
            //}
        }

        public List<ulong> Messages { get; set; } = new();
        public System.Timers.Timer Timer { get; set; } = new();
        public Stopwatch Stopwatch { get; set; } = new();
        private void ConfigureConsumer()
        {
            Timer.Start();
            Timer.Interval = 1000;
            Timer.Elapsed += Timer_Elapsed;
            Stopwatch.Start();
            try
            {
                IConnection connection = Factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.BasicQos(0, 5000, false);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    Messages.Add(ea.DeliveryTag);
                    //Console.WriteLine(ea.DeliveryTag);
                    //if (false)
                    //Console.WriteLine("Ack for {0}", Encoding.UTF8.GetString(ea.Body.ToArray()));
                    //    channel.BasicNack(ea.DeliveryTag, false, true);
                    if (Messages.Count == 5000)
                    {
                        for (int i = 0; i < Messages.Count; i++)
                        {
                            channel.BasicAck(Messages[i], true);
                        }
                        Stopwatch.Restart();
                        Messages = new();
                    }
                };
                Model = channel;
                channel.BasicConsume(queue: "AsiaTech_ReadyToSendMessages", autoAck: false, consumer: consumer);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to Configure Consumer, Unhandled Execption.");
            }
        }
        public IModel Model { get; set; }
        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (Stopwatch.ElapsedMilliseconds >= 10000)
            {
                for (int i = 0; i < Messages.Count; i++)
                {
                    Model.BasicAck(Messages[i], true);
                }
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Ack for {0}", Messages.Count);
                Console.BackgroundColor = ConsoleColor.Black;

                Messages = new();
                Stopwatch.Restart();
            }
        }

        public class ServiceSettingsConfig_Model
        {
            public string RabbitMqHost { get; set; }
            public string RabbitMqUser { get; set; }
            public string RabbitMqPass { get; set; }
            public int RabbitMqPort { get; set; }
        }
    }
}