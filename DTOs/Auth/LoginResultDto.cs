using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_2.DTOs.Auth
{
    public class LoginResultDto
    {
        public bool Successfull { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
    }
}