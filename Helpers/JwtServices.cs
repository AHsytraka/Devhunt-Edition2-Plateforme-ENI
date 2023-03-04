using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Devhunt.Helpers;

public class JwtServices
{
    private string secureKey = "JWT security Key for Web Token 098";
    //GENERATE TOKEN
    public string Generator(string nmat)
    {
        // Security data
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        var jwtHeader = new JwtHeader(credentials);

        var jwtPayload = new JwtPayload(nmat.ToString(), null, null, null, DateTime.Now.AddDays(7));
        var securityToken = new JwtSecurityToken(jwtHeader, jwtPayload);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    //CHECK TOKEN VALIDATION
    public JwtSecurityToken Checker(string jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secureKey);
        tokenHandler.ValidateToken(
            jwt,
            new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
            },
            out SecurityToken validateToken
        );
        return (JwtSecurityToken)validateToken;
    }
}