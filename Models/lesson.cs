using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Devhunt.Models;

public class Lesson
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LessonID { get; set; }
    public string? Description { get; set; }
    public string? Niveau { get; set; }
    public byte[]? Fichier { get; set; }
    public string? Nmat { get; set; }
    [ForeignKey("Nmat")]
    [JsonIgnore]
    public virtual User? User { get; set; }
}