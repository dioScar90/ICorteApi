// namespace ICorteApi.Domain.Base;

// public readonly struct Error(ErrorType type, string message) : IEquatable<Error>
// {
//     public ErrorType Type { get; } = type;
//     public string Message { get; } = message;

//     public static readonly Error None = new(ErrorType.None, string.Empty);
//     public static readonly Error NullValue = new(ErrorType.NullValue, "The value is null.");
//     public static readonly Error NotFound = new(ErrorType.NotFound, "The requested resource was not found.");
//     public static readonly Error BadRequest = new(ErrorType.BadRequest, "The request was invalid.");
//     public static readonly Error InvalidInput = new(ErrorType.InvalidInput, "The input provided was invalid.");
//     public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to perform this action.");
//     public static readonly Error Forbidden = new(ErrorType.Forbidden, "You do not have permission to perform this action.");
//     public static readonly Error Conflict = new(ErrorType.Conflict, "A conflict occurred with the current state of the resource.");
//     public static readonly Error InternalServerError = new(ErrorType.InternalServerError, "An internal server error occurred.");
//     public static readonly Error ServiceUnavailable = new(ErrorType.ServiceUnavailable, "The service is currently unavailable.");
//     public static readonly Error Timeout = new(ErrorType.Timeout, "The operation timed out.");
//     public static readonly Error ValidationError = new(ErrorType.ValidationError, "One or more validation errors occurred.");
//     public static readonly Error DependencyFailure = new(ErrorType.DependencyFailure, "A dependency failed to respond or is unavailable.");
//     public static readonly Error ResourceExhausted = new(ErrorType.ResourceExhausted, "The resource limit has been exhausted.");
//     public static readonly Error RateLimitExceeded = new(ErrorType.RateLimitExceeded, "The rate limit has been exceeded.");
//     public static readonly Error NotImplemented = new(ErrorType.NotImplemented, "The requested functionality is not implemented.");
//     public static readonly Error BadGateway = new(ErrorType.BadGateway, "Received an invalid response from the upstream server.");
//     public static readonly Error GatewayTimeout = new(ErrorType.GatewayTimeout, "The upstream server did not respond in time.");

//     public override readonly string ToString() => Message;

//     public override readonly bool Equals(object? obj) => obj is Error error && Equals(error);

//     public readonly bool Equals(Error other) => Type == other.Type && Message == other.Message;

//     public override readonly int GetHashCode() => HashCode.Combine((int)Type, Message);

//     public static bool operator ==(Error left, Error right) => left.Equals(right);
//     public static bool operator !=(Error left, Error right) => !(left == right);
// }

// public enum ErrorType
// {
//     None,
//     NullValue,
//     NotFound,
//     BadRequest,
//     InvalidInput,
//     Unauthorized,
//     Forbidden,
//     Conflict,
//     InternalServerError,
//     ServiceUnavailable,
//     Timeout,
//     ValidationError,
//     DependencyFailure,
//     ResourceExhausted,
//     RateLimitExceeded,
//     NotImplemented,
//     BadGateway,
//     GatewayTimeout
// }
