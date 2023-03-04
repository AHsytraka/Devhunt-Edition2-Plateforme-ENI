using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Devhunt.Models;

public class ComBox
{
    [Key]
    public int ComBoxID {get; set;}
    public string? Comms {get; set;}
    public byte[]? ComImage { get; set;}
    public bool IfValid {get; set;}
    public string? Ncommentateur {get; set;}
    public int PubID {get; set;}
    [ForeignKey("PubID")]
    [JsonIgnore]
    public virtual Pub? Pub {get; set;}
    public string? Nmat {get; set;}
    [ForeignKey("Nmat")]
    [JsonIgnore]
    public virtual User? User {get; set;}
}