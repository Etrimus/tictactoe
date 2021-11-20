using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TicTacToe.App.User;

namespace TicTacToe.Web.Authentication;

public class AuthService
{
    public string GetJwtToken(UserModel user)
    {
        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
                issuer: JwtOptions.ISSUER,
                audience: JwtOptions.AUDIENCE,
                notBefore: now,
                claims: new[] { new Claim(ClaimTypes.Name, user.Name) },
                expires: now.Add(TimeSpan.FromMinutes(JwtOptions.LIFETIME_IN_MINUTES)),
                signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}