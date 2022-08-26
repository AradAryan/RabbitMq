using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq
{
    internal class RabbitConsumer : IConsumer<Batch<PersonModel>>
    {
        public RabbitConsumer()
        {
        }

        public Task Consume(ConsumeContext<Batch<PersonModel>> context)
        {
            Console.WriteLine(context.Message.Count());

            return Task.CompletedTask;
        }

      
    }
}
