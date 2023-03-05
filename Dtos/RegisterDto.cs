namespace Devhunt.Dtos;

public class RegisterDto
{
    public string? Nmat { get; set; }
    public string? Username{ get; set; }
    public string? Parcour { get; set; }
    public string? Email { get; set; }
    public bool ConfirmedEmail {get; set;}
    public byte[]? Pdp { get; set; }
    public string? Mdp { get; set; }
}