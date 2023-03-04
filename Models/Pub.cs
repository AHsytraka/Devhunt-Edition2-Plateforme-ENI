using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Devhunt.Models;

public class Pub
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PubID { get; set; }
    public string? Description { get; set; }
    public byte[]? PubImg { get; set; }
    public bool ResolvedProblem { get; set; }
    
    public string? Nmat { get; set; }
    [ForeignKey("Nmat")]
    [JsonIgnore]
    public virtual User? User { get; set; }
}