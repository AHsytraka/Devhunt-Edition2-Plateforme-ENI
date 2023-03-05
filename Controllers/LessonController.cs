using Microsoft.AspNetCore.Mvc;
using Devhunt.Models;
using Devhunt.Data;
using Devhunt.Dtos;
using Devhunt.Helpers;
using BCrypt;
using Microsoft.EntityFrameworkCore;

namespace Devhunt.Controllers.LessonController;

#nullable disable

[Route("[Controller]")]
[ApiController]
public class ApiController: Controller
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

    /******************CREATE LESSON*****************/


    [HttpPost("CreateLesson")]
    public IActionResult CreateLesson(LessonDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string Nmat = (token.Issuer);

        var lesson = new Lesson
        {
            Description = dto.Description,
            Fichier = dto.Fichier,
            Niveau = dto.Niveau,
            Nmat = Nmat,
        };

        _lessonRepository.CreateLesson(lesson);

        return Created("Document crée avec succès",_lessonRepository.CreateLesson(lesson));
    }

    /******************DELETE LESSON*****************/

    [HttpPost("DeleteLesson")]
    public IActionResult DeleteLesson(LessonDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string userId = (token.Issuer);

        int IdLesson = dto.LessonID;

        var LessonUId = _context.Lessons.Where(b=>b.Nmat == userId).Select(b=>b.Nmat).FirstOrDefault();

        var LessonToRemove = _context.Lessons.FirstOrDefault(b => b.LessonID == IdLesson);

        if(LessonToRemove != null && userId == LessonUId)
        {
            _context.Lessons.Remove(LessonToRemove);
            _context.SaveChanges();
            return Ok(new {message ="Leçon supprimer avec succès"});
        }
        else {
            return Unauthorized();
        }
    }
}