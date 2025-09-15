using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_2.DTOs.Common
{
    public class ComandResultDto
    {
        public bool Successfull { get; set; }
        public string? Massage { get; set; }
    }
}