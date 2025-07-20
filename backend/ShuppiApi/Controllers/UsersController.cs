using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShuppiApi.Data;
using ShuppiApi.Models;
using ShuppiApi.Dtos;
using BCrypt.Net;

namespace ShuppiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            return BadRequest("このメールアドレスはすでに使われています。");
        }

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "登録に成功しました。" });
    }
}
