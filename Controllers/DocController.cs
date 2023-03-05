using Microsoft.AspNetCore.Mvc;
using Devhunt.Models;
using Devhunt.Data;
using Devhunt.Dtos;
using Devhunt.Helpers;
using BCrypt;
using Microsoft.EntityFrameworkCore;

namespace Devhunt.Controllers.DocController;

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

    public ApiController (
        AppDbContext context,
        JwtServices jwtService,
        IPubRepository pubRepository,
        IDocRepository docRepository,
        IUserRepository userRepository
    ) {
        _context = context;
        _jwtService = jwtService;
        _pubRepository = pubRepository;
        _docRepository = docRepository;
        _userRepository = userRepository;
    }

    /******************CREATE DOC*****************/


    [HttpPost("CreateDoc")]
    public IActionResult CreateDoc(DocDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string Nmat = (token.Issuer);

        var doc = new Document
        {
            Description = dto.Description,
            Fichier = dto.Fichier,
            Nmat = Nmat,
        };

        _docRepository.CreateDoc(doc);

        return Created("Document crée avec succès",_docRepository.CreateDoc(doc));
    }

    /******************DELETE DOC*****************/

    [HttpPost("DeleteDoc")]
    public IActionResult DeleteDoc(DocDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string userId = (token.Issuer);

        int IdDoc = dto.DocID;

        var DocUId = _context.Documents.Where(b=>b.Nmat == userId).Select(b=>b.Nmat).FirstOrDefault();

        var DocToRemove = _context.Documents.FirstOrDefault(b => b.DocID == IdDoc);

        if(DocToRemove != null && userId == DocUId)
        {
            _context.Documents.Remove(DocToRemove);
            _context.SaveChanges();
            return Ok(new {message ="Document supprimer avec succès"});
        }
        else {
            return Unauthorized();
        }
    }
}