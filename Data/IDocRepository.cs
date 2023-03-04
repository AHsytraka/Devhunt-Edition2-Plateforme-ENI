using Devhunt.Models;

namespace Devhunt.Data;

public interface IDocRepository
{
    Document CreateDoc(Document document);
}