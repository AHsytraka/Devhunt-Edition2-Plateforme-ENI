using Devhunt.Models;

namespace Devhunt.Data;

public interface IPubRepository
{
    Pub CreatePub(Pub publication);
}