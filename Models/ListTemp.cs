using System.ComponentModel.DataAnnotations;

namespace Devhunt.Models;

public class ListTemp
{
    [Key]
    public int LId { get; set; }
    [MinLength(4)]
    [MaxLength(7)]
    public string? Nmat { get; set; }
    
    [MinLength(2)]
    [MaxLength(3)]
    public string? Parcour { get; set; }
}