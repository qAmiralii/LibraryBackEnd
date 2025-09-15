using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DBcontextes;
using Backend.Entities;
using Backend_2.DTOs.Common;
using Backend_2.DTOs.Members;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_2.Endpoints
{
    public static class MemberEndpoints
    {
        public static void MapMemberEndPoints(this WebApplication app)
        {
            app.MapGet("v1/members/list", async ([FromServices] LibraryDB db) =>
            {
                var result = await db.Members.Select(x => new MemberListDto
                {
                    Id = x.Guid,
                    FirstName = x.FirstName,
                    Gender = x.Gender,
                    LastName = x.LastName
                }).ToListAsync();
                return result;
            });
            app.MapPost("v1/member/create", async (
                [FromServices] LibraryDB db,
                [FromBody] MemberAddDto memberAddDto) =>
            {
                var member = new Member
                {
                    FirstName = !string.IsNullOrEmpty(memberAddDto.FirstName.Trim()) ? memberAddDto.FirstName : "بی نام",
                    LastName = !string.IsNullOrEmpty(memberAddDto.LastName.Trim()) ? memberAddDto.LastName : "بی فامیلی",
                    Gender = memberAddDto.Gender,
                };
                await db.Members.AddAsync(member);
                await db.SaveChangesAsync();
                return new ComandResultDto
                {
                    Successfull = true,
                    Massage = "member added!"
                };
            });
            app.MapPut("v1/member/update{id}", async (
                [FromServices] LibraryDB db,
                string guid,
                MemberUpdateDto memberUpdateDto
            ) =>
            {
                var search = await db.Members.FirstOrDefaultAsync(x => x.Guid == guid);
                if (search == null)
                {
                    return new ComandResultDto
                    { Successfull = false, Massage = "member not found" };

                }
                search.FirstName = memberUpdateDto.FirstName ?? search.FirstName;
                search.LastName = memberUpdateDto.LastName ?? search.LastName;
                search.Gender = memberUpdateDto.Gender;
                await db.SaveChangesAsync();
                return new ComandResultDto { Successfull = true, Massage = "book updated!" };

            });
            app.MapDelete("v1/member/remove{id}", (
                
            ) =>
            {

            });
        }
    }
}