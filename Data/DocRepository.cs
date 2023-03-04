using Devhunt.Models;

namespace Devhunt.Data;

public class DocRepository : IDocRepository
{
    //Dependency Injection
    private readonly AppDbContext _context;
    public DocRepository(AppDbContext context)
    {
        _context = context;
    }
    public Document CreateDoc(Document document)
    {
        _context.Documents.Add(document);
        document.DocID = _context.SaveChanges();
        return document;
    }
}