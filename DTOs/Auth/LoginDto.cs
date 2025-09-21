using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_2.DTOs.Auth
{
    public class LoginDto
    {
        public required string Username { get; set; }
        public string? Password { get; set; }
    }
}