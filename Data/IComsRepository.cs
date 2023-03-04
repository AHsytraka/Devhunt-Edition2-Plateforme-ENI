using Devhunt.Models;

namespace Devhunt.Data;

public interface IComsRepository
{
    Commentaire CreateCom(Commentaire comBox);
}