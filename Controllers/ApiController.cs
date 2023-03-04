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
            var AllPub = _context.Pubs.FromSqlRaw("SELECT * FROM Pubs ORDER BY PubID DESC").ToList();

            var pubUNmat = _context.Pubs.Where(b => b.Nmat == Nmat).Select(b => b.Nmat).FirstOrDefault();

            if (user != null && AllPub != null && Nmat == pubUNmat)
            {
                return Ok(AllPub);
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

       /******************CREACTE PUB*****************/

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