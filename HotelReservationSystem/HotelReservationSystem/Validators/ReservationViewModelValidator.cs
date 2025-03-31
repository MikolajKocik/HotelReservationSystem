using FluentValidation;
using HotelReservationSystem.Models.ViewModels;

namespace HotelReservationSystem.Validators
{
    public class ReservationViewModelValidator : AbstractValidator<ReservationViewModel>
    {
        public ReservationViewModelValidator()
        {
            RuleFor(x => x.ArrivalDate)
                .NotEmpty().WithMessage("Data przyjazdu jest wymagana.")
                .LessThan(x => x.DepartureDate).WithMessage("Data przyjazdu musi być wcześniejsza niż wyjazdu.");

            RuleFor(x => x.DepartureDate)
                .NotEmpty().WithMessage("Data wyjazdu jest wymagana.");

            RuleFor(x => x.GuestFirstName)
                .NotEmpty().WithMessage("Imię jest wymagane.")
                .MaximumLength(50);

            RuleFor(x => x.GuestLastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.")
                .MaximumLength(50);

            RuleFor(x => x.GuestEmail)
                .NotEmpty().WithMessage("Email jest wymagany.")
                .EmailAddress().WithMessage("Niepoprawny format adresu email.");

            RuleFor(x => x.GuestPhoneNumber)
                .NotEmpty().WithMessage("Telefon jest wymagany.")
                .Matches(@"^\d{9}$").WithMessage("Telefon musi mieć 9 cyfr.");

            RuleFor(x => x.RoomId)
                .GreaterThan(0).WithMessage("Wybór pokoju jest wymagany.");
        }
    }
}
