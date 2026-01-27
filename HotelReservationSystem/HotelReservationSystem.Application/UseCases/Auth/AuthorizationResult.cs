namespace HotelReservationSystem.Application.UseCases.Auth;

/// <summary>
/// Represents the result of an authorization operation
/// </summary>
public class AuthorizationResult
{
    /// <summary>
    /// Gets a value indicating whether the authorization was successful
    /// </summary>
    public bool Succeeded { get; }

    /// <summary>
    /// Gets the failure reason if authorization failed
    /// </summary>
    public AuthorizationFailure? Failure { get; }

    private AuthorizationResult(bool succeeded, AuthorizationFailure? failure = null)
    {
        Succeeded = succeeded;
        Failure = failure;
    }

    /// <summary>
    /// Creates a successful authorization result
    /// </summary>
    public static AuthorizationResult Success() => new(true);

    /// <summary>
    /// Creates a failed authorization result
    /// </summary>
    public static AuthorizationResult Failed(AuthorizationFailure failure) => new(false, failure);

    /// <summary>
    /// Creates a failed authorization result with a reason
    /// </summary>
    public static AuthorizationResult Failed(string failureMessage) =>
        new(false, new AuthorizationFailure { Message = failureMessage });
}

/// <summary>
/// Represents an authorization failure
/// </summary>
public class AuthorizationFailure
{
    /// <summary>
    /// Gets or sets the failure message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the failure reasons
    /// </summary>
    public IEnumerable<string> FailureReasons { get; set; } = Array.Empty<string>();
}