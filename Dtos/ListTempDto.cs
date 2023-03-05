using System.ComponentModel.DataAnnotations;

namespace Devhunt.Dtos;

public class ListTempDto
{
    public int LId { get; set; }
    public string? Nmat { get; set; }
    public string? Parcour { get; set; }
}