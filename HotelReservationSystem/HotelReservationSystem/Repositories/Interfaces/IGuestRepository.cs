using HotelReservationSystem.Models.Domain;

namespace HotelReservationSystem.Repositories.Interfaces
{
    public interface IGuestRepository
    {
        Task<List<Payment>> GetTransactions();
    }

}
