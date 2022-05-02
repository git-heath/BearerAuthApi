namespace BearerAuthApi;

/// <summary>
/// Hard coded account information. In a real project the <see cref="SecretKey"/>
/// would be securely stored in a vault
/// </summary>
public static class AccountDetails
{
    public const string Username = "bob";
    public const string PasswordHash = "o0xmlOXgrVmvkBZ7/GcK6M92pxX5T/GbLmn54O/BYNA=";
    public static readonly byte[] Salt = Convert.FromBase64String("wcuD021FUZuNmEvoI0/5Lg==");
    public const string SecretKey = "E2vprpLWqe4eZV83AjXrTfm8BQvbH7zD";
}
