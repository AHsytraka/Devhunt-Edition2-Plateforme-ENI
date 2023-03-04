namespace Devhunt.Dtos;

public class ComsDto
{
    public int ComsID {get; set;}
    public string? Comms {get; set;}
    public byte[]? ComImage { get; set;}
    public bool IfValidComs {get; set;}
    public string? Ncommentateur {get; set;}
    public int PubID {get; set;}
}