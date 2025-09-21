using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Entities.Base;

namespace Backend_2.Entities
{
    public class Admin : Thing
    {
        public required string Username { get; set; }
        public string? Password { get; set; }
        public string? Fullname { get; set; }
    }
}