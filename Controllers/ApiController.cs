using Microsoft.AspNetCore.Mvc;
using Devhunt.Models;
using Devhunt.Data;
using Devhunt.Dtos;
using Devhunt.Helpers;
using BCrypt;
using Microsoft.EntityFrameworkCore;

namespace Devhunt.Controllers;

#nullable disable

[Route("[Controller]")]
[ApiController]
public class ApiController: Controller
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


    /******************USER API ************************/

    /******************LOGIN API***********************/

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

        return Ok(new { message = "succeded" });
    }

    /*************** LOGOUT API ******************/

    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwToken");
        return Ok(new { message = "Vous êtes déconnecter" });
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
            ReactCount = dto.ReactCount,
            //false = 0; true = 1;
            ResolvedProblem = dto.ResolvedProblem,
            Nmat = Nmat
        };

        _pubRepository.CreatePub(pub);

        return Created("created publication successfully",_pubRepository.CreatePub(pub));
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
            var recentPub = _context.Pubs.FromSqlRaw("SELECT * FROM Pubs WHERE PubID= (SELECT MAX(PubID) FROM Pubs)").ToList();

            var pubUNmat = _context.Pubs.Where(b => b.Nmat == Nmat).Select(b => b.Nmat).FirstOrDefault();

            if (user != null && recentPub != null && Nmat == pubUNmat)
            {
                return Ok(recentPub);
            }
            else 
            {
                return Ok(new { message = "Aucun publication n'a été trouver" });
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

        var productToRemove = _context.Pubs.FirstOrDefault(b => b.PubID == IdPub);

        if(productToRemove != null && userId == pubUId)
        {
            _context.Pubs.Remove(productToRemove);
            _context.SaveChanges();
            return Ok(new {message ="Publication supprimer avec succès"});
        }
        else {
            return Unauthorized();
        }
    }
}