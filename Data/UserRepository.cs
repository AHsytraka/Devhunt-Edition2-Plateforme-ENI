using Devhunt.Models;

namespace Devhunt.Data;

#nullable disable
//UserRepository Class implements IUserRepository Interface.
// Interface contains the signature of the methods to be implemented(used) in the class
// Class (install) use the method defined in the interface
public class UserRepository : IUserRepository
{
    //Dependency Injection
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    //Implementation on CreateUser in UserRepository
    // public User GetByEmail(string email)
    // {
    //     return _context.Users.FirstOrDefault(u => u.Email == email);
    // }

    public User GetByNmat(string Nmat)
    {
        return _context.Users.FirstOrDefault(u => u.Nmat == Nmat);
    }
}