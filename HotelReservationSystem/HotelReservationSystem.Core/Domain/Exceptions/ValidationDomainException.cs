namespace HotelReservationSystem.Core.Domain.Exceptions;

public sealed class ValidationDomainException : Exception
{
    public ValidationDomainException(string message) : base(message) { }
}
