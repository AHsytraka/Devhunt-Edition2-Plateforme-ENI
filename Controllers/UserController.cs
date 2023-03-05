using Microsoft.AspNetCore.Mvc;
using Devhunt.Data;
using Devhunt.Models;
using Devhunt.Dtos;
using Devhunt.Helpers;

namespace Devhunt.Controllers.UserController;

#nullable disable

[Route("[Controller]")]
[ApiController]
public class ApiController : Controller
{
    private readonly AppDbContext _context;
    private readonly JwtServices _jwtService;
    private readonly IPubRepository _pubRepository;
    private readonly IDocRepository _docRepository;
    private readonly IUserRepository _userRepository;
    private readonly IComsRepository _comsRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly IReactionRepository _reactionRepository;

    public ApiController (
        AppDbContext context,
        JwtServices jwtService,
        IPubRepository pubRepository,
        IDocRepository docRepository,
        IUserRepository userRepository,
        IComsRepository comsRepository,
        ILessonRepository lessonRepository,
        IReactionRepository reactionRepository
    ) {
        _context = context;
        _jwtService = jwtService;
        _pubRepository = pubRepository;
        _docRepository = docRepository;
        _userRepository = userRepository;
        _comsRepository = comsRepository;
        _lessonRepository = lessonRepository;
        _reactionRepository = reactionRepository;
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
        // if (!BCrypt.Net.BCrypt.Verify(dto.Mdp, user.Mdp))
        // {
        //     return BadRequest(new { message = "Mot de passe non valide" });
        // }

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