using FluentValidation;
using HotelReservationSystem.Application.Dtos.Reservation;

namespace HotelReservationSystem.Application.Validators;

public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
{
    public CreateReservationDtoValidator()
    {
        RuleFor(x => x.ArrivalDate)
            .NotEmpty().WithMessage("Data przyjazdu jest wymagana.")
            .LessThan(x => x.DepartureDate).WithMessage("Data przyjazdu musi być wcześniejsza niż wyjazdu.");

        RuleFor(x => x.DepartureDate)
            .NotEmpty().WithMessage("Data wyjazdu jest wymagana.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imię jest wymagane.")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany.")
            .EmailAddress().WithMessage("Niepoprawny format adresu email.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon jest wymagany.")
            .Matches(@"^\d{9}$").WithMessage("Telefon musi mieć 9 cyfr.");

        RuleFor(x => x.RoomId)
            .GreaterThan(0).WithMessage("Wybór pokoju jest wymagany.");
    }
}
