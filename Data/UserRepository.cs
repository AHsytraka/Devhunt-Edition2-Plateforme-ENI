using System.Security.Claims;
using Devhunt.Models;
using Microsoft.AspNetCore.Identity;

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

    public User CreateUser(User user)
    {
        //adding User and saving it through db context
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
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