using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Devhunt.Models;

public class Commentaire
{
    [Key]
    public int ComsID {get; set;}
    public string? Comms {get; set;}
    public byte[]? ComImage { get; set;}
    public bool IfValidComs {get; set;}
    public string? Ncommentateur {get; set;}
    public int PubID {get; set;}
    [ForeignKey("PubID")]
    [JsonIgnore]
    public virtual Pub? Pub {get; set;}
}