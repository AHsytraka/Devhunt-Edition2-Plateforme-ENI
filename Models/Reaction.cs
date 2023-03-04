using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Devhunt.Models;

public class Reaction
{
    [Key]
    public int ReactID { get; set;}
    public String? Nreacteur {get; set;}
    public bool Reacted {get; set;}
    public int PubID {get; set;}
    [ForeignKey("PubID")]
    [JsonIgnore]
    public virtual Pub? Pub {get; set;}

}