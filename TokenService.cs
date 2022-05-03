using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Text;

namespace BearerAuthApi;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _securityKey;
    private readonly SigningCredentials _credentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public TokenService()
    {
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccountDetails.SecretKey));

        if (_securityKey.KeySize < SymmetricSignatureProvider.DefaultMinimumSymmetricKeySizeInBits)
        {
            throw new SecurityException($"The keysize in bits {_securityKey.KeySize} is less than the minimum length of {SymmetricSignatureProvider.DefaultMinimumSymmetricKeySizeInBits}");
        }
        _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public string IssueToken()
    {
        var token = new JwtSecurityToken(
                                             expires: DateTime.UtcNow.AddMinutes(2),
                                             signingCredentials: _credentials);

        return _jwtSecurityTokenHandler.WriteToken(token);
    }
}
