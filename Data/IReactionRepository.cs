using Devhunt.Models;

namespace Devhunt.Data;

public interface IReactionRepository
{
    Reaction CreateReaction(Reaction reaction);
    Reaction GetByReactId(int id);
}