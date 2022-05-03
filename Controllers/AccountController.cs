using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.RateLimit;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BearerAuthApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{                   
    private readonly ISyncPolicy _rateLimitPolicy;
    private readonly ITokenService _tokenService;

    public AccountController(ISyncPolicy rateLimitPolicy, ITokenService tokenService)
    {                  
        _rateLimitPolicy = rateLimitPolicy;
        _tokenService = tokenService;
    }
    
    [HttpGet]
    [Route("login")]
    [SwaggerOperation(Summary = "Generates a JWT token from a user/password combination which is valid for 2 minutes")]
    public ActionResult<string> Login([SwaggerParameter(Required = true, Description ="bob")]string username, [SwaggerParameter(Required = true, Description = "4UKqwtdNRnFhhnLW")]string password)
    {        
        try
        {           
            // Simple rate limiting policy to avoid brute force attacks
            return _rateLimitPolicy.Execute<ActionResult<string>>(() =>
            {
                return AreCredentialsCorrect(username, password) ? Ok(_tokenService.IssueToken()) : Unauthorized();
            });            
        }
        catch (RateLimitRejectedException)
        {                        
            return StatusCode((int)HttpStatusCode.TooManyRequests);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }        
    }

    [HttpGet]
    [Route("protected-endpoint")]
    [SwaggerOperation(Summary = "API endpoint protected by a JWT token. If token is missing, invalid or expired 401 is returned.")]
    [Authorize]
    public ActionResult<string> IsAllowed() => "Success";

    private static bool AreCredentialsCorrect(string username, string password) => 
        string.Equals(username, AccountDetails.Username, StringComparison.OrdinalIgnoreCase) &&
        HashPassword(password) == AccountDetails.PasswordHash;

    private static string HashPassword(string password) => 
        Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: AccountDetails.Salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
}
