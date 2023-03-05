using Devhunt.Models;

namespace Devhunt.Data;

public interface IUserRepository
{
    // User GetByEmail(string email);
    User CreateUser(User user);
    User GetByNmat(string Nmat);
}