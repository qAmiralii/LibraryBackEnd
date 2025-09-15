using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Entities.Base;
using Backend.Enum;

namespace Backend.Entities
{
    public class Member : Thing
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Gender Gender { get; set; }
    }
}