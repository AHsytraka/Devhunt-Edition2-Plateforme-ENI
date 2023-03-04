namespace Devhunt.Dtos;

public class LessonDto
{
    public int LessonID { get; set; }
    public string? Description { get; set; }
    public string? Niveau { get; set; }
    public byte[]? Fichier { get; set; }
    public string? Nmat { get; set; }
}