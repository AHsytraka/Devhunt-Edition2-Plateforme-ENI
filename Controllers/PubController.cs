using Microsoft.AspNetCore.Mvc;
using Devhunt.Data;
using Devhunt.Models;
using Devhunt.Dtos;
using Devhunt.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Devhunt.Controllers.PubController;

#nullable disable

[Route("[Controller]")]
[ApiController]
public class ApiController : Controller
{
    private readonly AppDbContext _context;
    private readonly JwtServices _jwtService;
    private readonly IPubRepository _pubRepository;
    private readonly IUserRepository _userRepository;

    public ApiController (
        AppDbContext context,
        JwtServices jwtService,
        IPubRepository pubRepository,
        IUserRepository userRepository
    ) {
        _context = context;
        _jwtService = jwtService;
        _pubRepository = pubRepository;
        _userRepository = userRepository;
    }

    /************** CREER PUB*****************/

    [HttpPost("registerPub")]
    public IActionResult RegisterPub(PubDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string Nmat = (token.Issuer);

        var pub = new Pub
        {
            Description = dto.Description,
            PubImg = dto.PubImg,
            //false = 0; true = 1;
            ResolvedProblem = dto.ResolvedProblem,
            Nmat = Nmat
        };

        _pubRepository.CreatePub(pub);

        return Created("Publication créer",_pubRepository.CreatePub(pub));
    }

    /************** PUBLIER PUB *****************/

    [HttpGet("publicate")]
    public IActionResult Publicate()
    {
        try
        {
            var jwt = Request.Cookies["jwToken"];
            var token = _jwtService.Checker(jwt);

            string Nmat = (token.Issuer);

            var user = _userRepository.GetByNmat(Nmat);
            var AllPub = _context.Pubs.FromSqlRaw("SELECT * FROM Pubs ORDER BY PubID DESC").ToList();

            var pubUNmat = _context.Pubs.Where(b => b.Nmat == Nmat).Select(b => b.Nmat).FirstOrDefault();

            if (user != null && AllPub != null && Nmat == pubUNmat)
            {
                return Ok(AllPub);
            }
            else 
            {
                return Ok(new { message = "Publication non trouver" });
            }

        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }

    /************** PUBLIER PUB *****************/

    [HttpPost("UpdatePub")]
    public IActionResult UpdatePub(PubDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string userId = (token.Issuer);

        var PubUid = _context.Pubs.Where(b => b.Nmat == userId).Select(b => b.Nmat).FirstOrDefault();

        // CHANGE TO RESPONSE VALUE FROM FRONTEND
        //RESPONSE FROM FROM
        int IdPub = 1;
        //Product id value in Produits table
        var PubId = _context.Pubs.Where(b => b.PubID == IdPub).Select(b => b.PubID).FirstOrDefault();


        //user Id must be equal to Pub's user id
        //product Id must be equal to the selected product's id from the frontend
        //user Id must be equal to user's id from the selected product row
        if(userId == PubUid && IdPub == PubId)
        {
            var productToUpdate = _context.Pubs.Find(IdPub);
            // Update the properties of the entity object
            productToUpdate.Description = dto.Description;
            productToUpdate.PubImg = dto.PubImg;
            productToUpdate.Nmat = userId;

            // Call SaveChanges to persist the changes to the database
            _context.SaveChanges();

            return Ok( new {message = "Publication modifier avec succès"});
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpPost("SolvedProblem")]
    public IActionResult SolvedProblem(PubDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string userId = (token.Issuer);

        var PubUid = _context.Pubs.Where(b => b.Nmat == userId).Select(b => b.Nmat).FirstOrDefault();

        // CHANGE TO RESPONSE VALUE FROM FRONTEND
        //RESPONSE FROM FROM
        int IdPub = 1;
        //Product id value in Produits table
        var PubId = _context.Pubs.Where(b => b.PubID == IdPub).Select(b => b.PubID).FirstOrDefault();


        //user Id must be equal to Pub's user id
        //product Id must be equal to the selected product's id from the frontend
        //user Id must be equal to user's id from the selected product row
        if(userId == PubUid && PubId == IdPub)
        {
            var productToUpdate = _context.Pubs.Find(IdPub);
            // Update the properties of the entity object
            productToUpdate.ResolvedProblem = dto.ResolvedProblem;

            // Call SaveChanges to persist the changes to the database
            _context.SaveChanges();

            return Ok( new {message = "Le problem a bien été résolu"});
        }
        else
        {
            return Unauthorized();
        }
    }

   /******************DELETE PRODUCT*****************/


    [HttpPost("DeletePub")]
    public IActionResult DeleteProduct(PubDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string userId = (token.Issuer);

        int IdPub = dto.PubID;

        var pubUId = _context.Pubs.Where(b=>b.Nmat == userId).Select(b=>b.Nmat).FirstOrDefault();

        var pubToRemove = _context.Pubs.FirstOrDefault(b => b.PubID == IdPub);

        if(pubToRemove != null && userId == pubUId)
        {
            _context.Pubs.Remove(pubToRemove);
            _context.SaveChanges();
            return Ok(new {message ="Publication supprimer avec succès"});
        }
        else {
            return Unauthorized();
        }
    }
}