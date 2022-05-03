namespace BearerAuthApi;

public interface ITokenService
{
    /// <summary>
    /// Issue a new access token
    /// </summary>
    /// <returns>An access token</returns>
    string IssueToken();
}
