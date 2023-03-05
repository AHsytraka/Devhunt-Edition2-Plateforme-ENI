using Microsoft.AspNetCore.Mvc;
using Devhunt.Data;
using Devhunt.Models;
using Devhunt.Dtos;
using Devhunt.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Devhunt.Controllers.ReactController;

#nullable disable

[Route("[Controller]")]
[ApiController]
public class ApiController : Controller
{
    private readonly AppDbContext _context;
    private readonly JwtServices _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IReactionRepository _reactionRepository;

    public ApiController (
        AppDbContext context,
        JwtServices jwtService,
        IUserRepository userRepository,
        IReactionRepository reactionRepository
    ) {
        _context = context;
        _jwtService = jwtService;
        _userRepository = userRepository;
        _reactionRepository = reactionRepository;
    }

    /******************REACT ON PUBLICATION*****************/


    [HttpPost("React")]
    public IActionResult React(ReactDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string Nmat = (token.Issuer);

        int idPub = dto.PubID;
        int idReact = dto.ReactID;
        bool reactState = dto.Reacted;

        var reactedPub = _context.Reactions
            .Where(
                b => b.PubID == idPub && b.ReactID == idReact && b.Reacted == reactState)
            .Select(
                b => b.Reacted
        ).FirstOrDefault();

        if(reactedPub == true)
        {
            var ReactionToRemove = _context.Reactions.FirstOrDefault(b => b.ReactID == idReact);
            _context.Reactions.Remove(ReactionToRemove);
            _context.SaveChanges();
            return Ok(new {message ="Reaction supprimer avec succès"});
        }
        else
        {
            var reaction = new Reaction
            {
                PubID = dto.PubID,
                Reacted = true,
                Nreacteur = Nmat
            };

            _reactionRepository.CreateReaction(reaction);

            return Created("Document crée avec succès",_reactionRepository.CreateReaction(reaction));
        }
    }


    /******************COUNT PUBLICATION REACTION*****************/

   [HttpGet("CountReact")]
    public IActionResult CountReact(ReactDto dto)
    {
        try
        {
            var jwt = Request.Cookies["jwToken"];
            var token = _jwtService.Checker(jwt);

            string Nmat = (token.Issuer);
            int idPub = dto.PubID;

            var user = _userRepository.GetByNmat(Nmat);

            var CountReaction = _context.Reactions.Where(b=>b.PubID == idPub).Count(b=>b.Reacted);

            if (user != null)
            {
                return Ok(CountReaction);
            }
            else
            {
                return Ok(new { message = "Impossible d'avoir la liste des reactions" });
            }

        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }
}