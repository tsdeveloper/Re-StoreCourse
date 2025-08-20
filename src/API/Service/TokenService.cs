using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Data;
using API.Entities;
using API.Entities.JWT;
using API.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Service;

public class TokenService
{
    private readonly UserManager<UserCustom> _userManager;
    private readonly RestoreCourseDbContext _context;
    private readonly JWTSettings _jwtSettings;

    public TokenService(UserManager<UserCustom> userManager, IOptions<JWTSettings> jwtSettings,
        RestoreCourseDbContext context)
    {
        _userManager = userManager;
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<string> GenerateToke(UserCustom user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.TokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.Expire),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public async Task<string> GenerateRefreshToken(int userId)
    {
        var refreshToken = new JWTRefreshToken
        {
            Token = Guid.NewGuid().ToString("N"),
            CreatedAt = DateTime.Now,
            ExpiredAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireRefreshToken),
            UserId = userId
        };
        _context.DbSet<JWTRefreshToken>().Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken.Token;
    }

    public async Task RevokeRefreshToken(string token)
    {
        var existingToken = await _context.DbSet<JWTRefreshToken>()
            .SingleOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);

        if (existingToken != null)
        {
            existingToken.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

}