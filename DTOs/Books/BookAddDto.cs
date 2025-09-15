using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_2.DTOs.Books
{
    public class BookAddDto
    {
        public string? Title { get; set; }
        public string? Writer { get; set; }
        public string? Publisher { get; set; }
        public double Price { get; set; }
    }
}