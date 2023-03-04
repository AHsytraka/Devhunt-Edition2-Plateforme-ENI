using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Devhunt.Models;

public class Document
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DocID { get; set; }
    public string? Description { get; set; }
    public int ReactCount { get; set; }
    public byte[]? Fichier { get; set; }
    public string? Commentaire { get; set; }
    public bool validResp { get; set; }
    public string? Nmat { get; set; }
    [ForeignKey("Nmat")]
    [JsonIgnore]
    public virtual User? User { get; set; }
}