using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Devhunt.Models;

public class User 
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Nmat { get; set; }

    [MaxLength(50)]
    public string? Username { get; set; }
    public string? Parcour { get; set; }
    public string? Email { get; set; }
    public byte[]? Pdp { get; set; }

    [MaxLength(255)]
    [JsonIgnore]
    public string? Mdp { get; set; }
}