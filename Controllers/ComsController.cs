using Microsoft.AspNetCore.Mvc;
using Devhunt.Data;
using Devhunt.Models;
using Devhunt.Dtos;
using Devhunt.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Devhunt.Controllers.ComsController;

#nullable disable

[Route("[Controller]")]
[ApiController]
public class ApiController : Controller
{
    private readonly AppDbContext _context;
    private readonly JwtServices _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IComsRepository _comsRepository;

    public ApiController (
        AppDbContext context,
        JwtServices jwtService,
        IComsRepository comsRepository,
        IUserRepository userRepository
    ) {
        _context = context;
        _jwtService = jwtService;
        _comsRepository = comsRepository;
        _userRepository = userRepository;
    }

    /******************CREACTE COMS*****************/

    [HttpPost("CreateComs")]
    public IActionResult CreateComs(ComsDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string Nmat = (token.Issuer);
        int idPub = 1;
        var coms = new Commentaire
        {
            Comms = dto.Comms,
            ComImage = dto.ComImage,
            //false = 0; true = 1; (mysql)
            //false = false; true = true; (Json)
            IfValidComs = dto.IfValidComs,
            Ncommentateur = Nmat,
            PubID = idPub
        };

        _comsRepository.CreateCom(coms);

        return Created("created publication successfully",_comsRepository.CreateCom(coms));
    }

    /******************COMMENT PUB*****************/

    [HttpGet("PubComs")]
    public IActionResult PubComs(PubDto dto)
    {
        try
        {
            var jwt = Request.Cookies["jwToken"];
            var token = _jwtService.Checker(jwt);

            string Nmat = (token.Issuer);
            int idPub = dto.PubID;

            var user = _userRepository.GetByNmat(Nmat);

            var AllComs = _context.Commentaires.FromSqlRaw($"SELECT * FROM Commentaires WHERE PubID={idPub} ORDER BY ComsID DESC").ToList();

            var ComNmat = _context.Commentaires.Where(b => b.Ncommentateur == Nmat).Select(b => b.Ncommentateur).FirstOrDefault();

            var ComPubId = _context.Commentaires.Where(b => b.PubID == idPub).Select(b => b.PubID).FirstOrDefault();

            if (user != null && AllComs != null && Nmat == ComNmat && ComPubId == idPub)
            {
                return Ok(AllComs);
            }
            else
            {
                return Ok(new { message = "Impossible de commenter la publication" });
            }

        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }

    /******************UPDATE COMS*****************/

    [HttpPost("UpdateComs")]
    public IActionResult UpdateComs(ComsDto dto)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string userId = (token.Issuer);

        var PubUid = _context.Pubs.Where(b => b.Nmat == userId).Select(b => b.Nmat).FirstOrDefault();

        // CHANGE TO RESPONSE VALUE FROM FRONTEND
        //RESPONSE FROM FROM
        int IdPub = 1;
        int IdCom = 6;
        //Product id value in Produits table
        var PubId = _context.Pubs.Where(b => b.PubID == IdPub).Select(b => b.PubID).FirstOrDefault();


        //user Id must be equal to Pub's user id
        //product Id must be equal to the selected product's id from the frontend
        //user Id must be equal to user's id from the selected product row
        if(userId == PubUid && IdPub == PubId)
        {
            var ComsToUpdate = _context.Commentaires.Find(IdCom);
            // Update the properties of the entity object
            ComsToUpdate.Comms = dto.Comms;
            ComsToUpdate.ComImage = dto.ComImage;
            ComsToUpdate.Ncommentateur = userId;

            // Call SaveChanges to persist the changes to the database
            _context.SaveChanges();

            return Ok( new {message = "Commentaire modifier avec succès"});
        }
        else
        {
            return Unauthorized();
        }
    }

    /******************DELETE COMS*****************/

    [HttpPost("DeleteComs")]
    public IActionResult DeleteComs(ComsDto dto1)
    {
        var jwt = Request.Cookies["jwToken"];
        var token = _jwtService.Checker(jwt);
        string userId = (token.Issuer);

        int IdComs = dto1.ComsID;

        var ComId = _context.Commentaires.Where(b=>b.ComsID == IdComs).Select(b=>b.ComsID).FirstOrDefault();
        var ComUid = _context.Commentaires.Where(b=>b.ComsID == IdComs).Select(b=>b.Ncommentateur).FirstOrDefault();

        var comsToRemove = _context.Commentaires.FirstOrDefault(b => b.ComsID == IdComs);

        if(comsToRemove != null && userId == ComUid)
        {
            _context.Commentaires.Remove(comsToRemove);
            _context.SaveChanges();
            return Ok(new {message ="Commentaire supprimer avec succès"});
        }
        else 
        {
            return Unauthorized();
        }
    }
}