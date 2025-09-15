using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_2.DTOs.Borrows
{
    public class BorrowAddDto
    {
        public required string BookGuid { get; set; }
        public required string MemberGuid { get; set; }
        
    }
}