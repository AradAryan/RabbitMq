using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq
{
    internal class PersonModel
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
    }
}
