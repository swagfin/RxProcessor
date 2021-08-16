using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxProcessor.Models
{
    public class SparesRegistration
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }

        public override string ToString()
        {
            return $"[{Name}], going for ${Price:N2}, Color: {Color}";
        }
    }
}
