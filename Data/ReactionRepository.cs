using Devhunt.Models;

namespace Devhunt.Data;

#nullable disable
public class ReactionRepository : IReactionRepository
{
    //Dependency Injection: Publication and CreatePub depends on ApplicationDbContext
    private readonly AppDbContext _context;
    public ReactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Reaction CreateReaction(Reaction reaction)
    {
        _context.Reactions.Add(reaction);
        reaction.ReactID = _context.SaveChanges();
        return reaction;
    }

    public Reaction GetByReactId(int id)
    {
        return _context.Reactions.FirstOrDefault(u => u.ReactID == id);
    }
}