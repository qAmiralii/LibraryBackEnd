using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.DBcontextes;
using Backend.Entities;
using Backend_2.DTOs.Books;
using Backend_2.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_2.Endpoints
{
    public static class BookEndpoints
    {
        public static void MapBookEndPoints(this WebApplication app)
        {
            app.MapGet("v1/books/list", async ([FromServices] LibraryDB db) =>
            {
                var result = await db.Books.Select(x => new BookListDto
                {
                    Id = x.Guid,
                    Title = x.Title,
                    Price = x.Price,
                    Publisher = x.Publisher,
                    Writer = x.Writer
                }).ToListAsync();
                return result;
            }).RequireAuthorization();
            app.MapPost("v1/books/create", async (
                [FromServices] LibraryDB db,
                [FromServices] ClaimsPrincipal claims,
                [FromBody] BookAddDto bookAddDto) =>
            {
                var adminGuid = claims.Claims.FirstOrDefault(x => x.Type == "guidd")?.Value;
                var admin = await db.Admins.FirstOrDefaultAsync(x => x.Guid == adminGuid);
                if (admin != null)
                {


                    var book = new Book
                    {
                        Title = !string.IsNullOrEmpty(bookAddDto.Title.Trim()) ? bookAddDto.Title : "بی عنوان",
                        Writer = bookAddDto.Writer,
                        Price = bookAddDto.Price,
                        Publisher = bookAddDto.Publisher,
                        Owner = admin
                    };
                    await db.Books.AddAsync(book);
                    await db.SaveChangesAsync();
                    return new ComandResultDto
                    {
                        Successfull = true,
                        Massage = "book created!"
                    };
                }
                return new ComandResultDto
                {
                    Successfull = false,
                    Massage = "Unkown User!"
                };
            });
            app.MapPut("v1/books/update{guid}", async ([FromServices] LibraryDB db, [FromRoute] string guid, BookUpdateDto bookUpdateDto) =>
            {
                var simple = await db.Books.FirstOrDefaultAsync(x => x.Guid == guid);
                if (simple == null)
                {
                    return new ComandResultDto
                    { Successfull = false, Massage = "book not found" };

                }
                simple.Price = bookUpdateDto.Price;
                simple.Publisher = bookUpdateDto.Publisher;
                simple.Title = bookUpdateDto.Title ?? "بی نام";
                simple.Writer = bookUpdateDto.Writer;
                await db.SaveChangesAsync();
                return new ComandResultDto { Successfull = true, Massage = "book updated!" };

            });
            app.MapDelete("v1/books/remove{guid}", async ([FromServices] LibraryDB db, [FromRoute] string guid) =>
            {
                var b = await db.Books.FirstOrDefaultAsync(x => x.Guid == guid);
                if (b == null)
                {
                    return new ComandResultDto { Successfull = false, Massage = "book not found" };
                }
                db.Books.Remove(b);
                await db.SaveChangesAsync();
                return new ComandResultDto { Successfull = true, Massage = "book removed!" };
            });

        }
    }
}