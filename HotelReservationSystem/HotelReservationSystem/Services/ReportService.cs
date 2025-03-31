using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Dtos;
using HotelReservationSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly HotelDbContext _context;

        public ReportService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<ReportDto> GenerateReportAsync(DateTime from, DateTime to)
        {
            var reservations = await _context.Reservations
                .Where(r => r.ArrivalDate >= from && r.DepartureDate <= to)
                .ToListAsync();

            return new ReportDto
            {
                TotalReservations = reservations.Count,
                Confirmed = reservations.Count(r => r.Status == "Potwierdzona"),
                TotalIncome = reservations.Sum(r => r.Payment?.Amount ?? 0)
            };
        }
    }

}
