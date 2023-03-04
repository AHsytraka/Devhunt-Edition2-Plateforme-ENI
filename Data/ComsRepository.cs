using Devhunt.Models;

namespace Devhunt.Data;

#nullable disable
public class ComsRepository : IComsRepository
{
    //Dependency Injection: Publication and CreatePub depends on ApplicationDbContext
    private readonly AppDbContext _context;
    public ComsRepository(AppDbContext context)
    {
        _context = context;
    }

    public Commentaire CreateCom(Commentaire comBox)
    {
        _context.Commentaires.Add(comBox);
        comBox.ComsID = _context.SaveChanges();
        return comBox;
    }
}