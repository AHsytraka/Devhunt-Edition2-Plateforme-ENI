using Devhunt.Models;

namespace Devhunt.Data;

public interface IUserRepository
{
    // User GetByEmail(string email);
    User GetByNmat(string Nmat);
}