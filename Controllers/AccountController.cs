using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Text;

namespace BearerAuthApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{               
    private readonly SymmetricSecurityKey _securityKey;
    private readonly SigningCredentials _credentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public AccountController()
    {          
        // In a real implementation this code would be isolated in a Singleton service
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccountDetails.SecretKey));

        if (_securityKey.KeySize < SymmetricSignatureProvider.DefaultMinimumSymmetricKeySizeInBits)
        {
            throw new SecurityException($"The keysize in bits {_securityKey.KeySize} is less than the minimum length of {SymmetricSignatureProvider.DefaultMinimumSymmetricKeySizeInBits}");
        }
        _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }
    
    [HttpGet]
    [Route("login")]
    [SwaggerOperation(Summary = "Generates a JWT token from a user/password combination which is valid for 2 minutes")]
    public ActionResult<string> Login([SwaggerParameter(Required = true, Description ="bob")]string username, [SwaggerParameter(Required = true, Description = "4UKqwtdNRnFhhnLW")] string password)
    {
        if (string.Equals(username, AccountDetails.Username, StringComparison.OrdinalIgnoreCase) &&
            HashPassword(password) == AccountDetails.PasswordHash)
        {
            var token = new JwtSecurityToken(
                                     expires: DateTime.UtcNow.AddMinutes(2),
                                     signingCredentials: _credentials);

            return Ok(_jwtSecurityTokenHandler.WriteToken(token));
        }
        else
            return Unauthorized();
    }

    [HttpGet]
    [Route("protected-endpoint")]
    [SwaggerOperation(Summary = "API endpoint protected by a JWT token. If token is missing, invalid or expired 401 is returned.")]
    [Authorize]
    public string IsAllowed()
    {
        return "Success";
    }

    private static string HashPassword(string password) => Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: AccountDetails.Salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
}
