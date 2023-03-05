using Microsoft.AspNetCore.Mvc;
using Devhunt.Data;
using Devhunt.Models;
using Devhunt.Dtos;
using Devhunt.Helpers;
using BCrypt;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using SendGrid;

namespace Devhunt.Controllers.UserController;

#nullable disable

[Route("[Controller]")]
[ApiController]
public class ApiController : Controller
{
    private readonly AppDbContext _context;
    private readonly JwtServices _jwtService;
    private readonly IUserRepository _userRepository;

    public ApiController (
        AppDbContext context,
        JwtServices jwtService,
        IUserRepository userRepository
    ) {
        _context = context;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    /************REGISTER**************/

    [HttpPost("Register")]
    public IActionResult Register(RegisterDto dto)
    {
        if(_context.Users.Any(u=>u.Email == dto.Email) && _context.Users.Any(u=>u.Nmat == dto.Nmat))
        {
            return BadRequest("Utilisateur déjà inscrit");
        }
        var user = new User
        {
            Nmat = dto.Nmat,
            Username = dto.Username,
            Parcour = dto.Parcour,
            ConfirmedEmail =false,
            Email = dto.Email,
            Pdp = dto.Pdp,
            Mdp = BCrypt.Net.BCrypt.HashPassword(dto.Mdp)
        };

        var result = _userRepository.CreateUser(user);

        return Ok("Utilisateur crée avec succès");
    }

    /*************LOGIN****************/

    [HttpPost("Login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _userRepository.GetByNmat(dto.Nmat);
        if (user == null)
        {
            return BadRequest(new { message = "Numéro matricule non valide ou inexistant" });
        }
        if (!BCrypt.Net.BCrypt.Verify(dto.Mdp, user.Mdp))
        {
            return BadRequest(new { message = "Mot de passe non valide" });
        }

        var jwt = _jwtService.Generator(user.Nmat);

        Response.Cookies.Append("jwToken", jwt, new CookieOptions
        {
            HttpOnly = true
        });

        return Ok(new { message = "Utilisateur connecter" });
    }

    /*************LOGIN****************/

    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwToken");
        return Ok(new { message = "Utilisateur déconnecter" });
    }
}