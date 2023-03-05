using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Devhunt.Models;

public class User 
{
    [Key]
    [Required]
    [MinLength(4)]
    [MaxLength(7)]
    public string? Nmat { get; set; }

    [MaxLength(50)]
    public string? Username { get; set; }
    [MaxLength(3)]
    public string? Parcour { get; set; }
    public string? Email { get; set; }
    public bool ConfirmedEmail {get; set;}
    public byte[]? Pdp { get; set; }
    [MaxLength(255)]
    [JsonIgnore]
    public string? Mdp { get; set; }
}