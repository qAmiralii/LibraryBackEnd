using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Entities.Base;

namespace Backend.Entities
{
    public class Borrow : Thing
    {
        public required Book Book { get; set; }
        public required Member Member { get; set; }
        public DateTime Borrowtime { get; set; }
        public DateTime? Returntime { get; set; }
    }
}