using Devhunt.Models;

namespace Devhunt.Data;

#nullable disable
public class PubRepository : IPubRepository
{
    //Dependency Injection: Publication and CreatePub depends on ApplicationDbContext
    private readonly AppDbContext _context;
    public PubRepository(AppDbContext context)
    {
        _context = context;
    }

    public Pub CreatePub(Pub pub)
    {
        _context.Pubs.Add(pub);
        pub.PubID = _context.SaveChanges();
        return pub;
    }
}