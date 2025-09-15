using System.Runtime.Intrinsics;
using System.Text;
using Backend.DBcontextes;
using Backend.Entities;
using Backend_2.DTOs.Books;
using Backend_2.DTOs.Borrows;
using Backend_2.DTOs.Common;
using Backend_2.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCors();
builder.Services.AddDbContext<LibraryDB>();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])) 
            };
        });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}
app.UseCors(policy =>
{
    policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin();
});
app.UseHttpsRedirection();

app.MapBookEndPoints();
app.MapMemberEndPoints();
app.MapPost("v1/borrows/create", async Task<ComandResultDto> ([FromServices] LibraryDB db, [FromBody] BorrowAddDto dto) =>
{
    var book = await db.Books.FirstOrDefaultAsync(x => x.Guid == dto.BookGuid) ?? throw new Exception("Book Not Found!");
    if (book == null)
    {
        return new ComandResultDto
        {
            Massage = "Book Not Found!",
            Successfull = false
        };
    }
    var member = await db.Members.FirstOrDefaultAsync(x => x.Guid == dto.MemberGuid) ?? throw new Exception("Member Not Found!");
    if (member == null)
    {
        return new ComandResultDto
        {
            Massage = "Member Not Found!",
            Successfull = false
        };
    }   
    var Borrow = new Borrow
    {
        Book = book,
        Member = member,
        Borrowtime = DateTime.Now
    };
    await db.Borrows.AddAsync(Borrow);
    await db.SaveChangesAsync();
    return new ComandResultDto
    {
        Successfull = true
    };
});

app.Run();