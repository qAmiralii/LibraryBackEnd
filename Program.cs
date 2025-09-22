using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics;
using System.Security.Claims;
using System.Text;
using Backend.DBcontextes;
using Backend.Entities;
using Backend_2.DTOs.Auth;
using Backend_2.DTOs.Books;
using Backend_2.DTOs.Borrows;
using Backend_2.DTOs.Common;
using Backend_2.Endpoints;
using Backend_2.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCors();
builder.Services.AddDbContext<LibraryDB>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "novin.ir",
            ValidAudience = "novin.ir",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a33965cb3fc02a37092214d029ca2e7b9bc71ab89c307cb58542f943633ed5270e1793404d76d9ccbb14c45c55a2f534310c502f962b7ebb720c81ec7e6a7485"))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


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
//   "Jwt": {
//     "SecretKey": "YourVeryLongSecretKeyHereThatIsAtLeast16Characters",
//     "Issuer": "YourIssuer",
//     "Audience": "YourAudience"
//   }
app.MapBookEndPoints();
app.MapMemberEndPoints();

app.MapPost("v1/auth/login", async Task<LoginResultDto> ([FromServices] LibraryDB db, [FromBody] LoginDto dto) =>
{
    if (!await db.Admins.AnyAsync())
    {
        await db.AddAsync(new Admin
        {
            Username = "admin",
            Password = "admin",
            Fullname = "amir ali"
        });
        await db.SaveChangesAsync();
    }
    var admin = await db.Admins.FirstOrDefaultAsync(x => x.Username == dto.Username && x.Password == dto.Password);

    if (admin != null)
    {
        var claims = new[]
        {
            new Claim("guid",admin.Guid),
            new Claim("Fullname",admin.Fullname??"no name")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a33965cb3fc02a37092214d029ca2e7b9bc71ab89c307cb58542f943633ed5270e1793404d76d9ccbb14c45c55a2f534310c502f962b7ebb720c81ec7e6a7485"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "novin.ir",
            audience: "novin.ir",
            claims: claims,
            expires: DateTime.Now.AddMonths(2),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);


        return new LoginResultDto
        {
            Successfull = true,
            Message = "Wellcome",
            Token = tokenString
        };
    }
    return new LoginResultDto
    {
        Message = "نام کاربری یا کمله عبور درست نیست",
        Successfull = false
    };
});

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