namespace HotelReservationSystem.Core.Domain.Exceptions;

public sealed class InvalidOperationDomainException : Exception
{
    public InvalidOperationDomainException(string message) : base(message) { }
}