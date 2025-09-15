using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Enum;

namespace Backend_2.DTOs.Members
{
    public class MemberListDto
    {
        public string? Id { get; set; }
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; }
    }
}