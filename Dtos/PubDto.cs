namespace Devhunt.Dtos;

public class PubDto
{
    public int PubID { get; set; }
    public string? Description { get; set; }
    public byte[]? PubImg { get; set; }
    public int ReactCount { get; set; }
    public bool ResolvedProblem { get; set; }
    public string? Nmat { get; set; }
}