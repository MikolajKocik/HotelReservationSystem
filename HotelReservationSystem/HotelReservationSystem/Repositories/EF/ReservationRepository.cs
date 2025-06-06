﻿using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Repositories.EF
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HotelDbContext _context;

        public ReservationRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(g => g.Guest)
                .Include(p => p.Payment)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GuestReservations(string userEmail)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Where(r => r.Guest.Email == userEmail)
                .ToListAsync();
        }

        public async Task<Reservation> GetById(int id)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(g => g.Guest)
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new InvalidOperationException("Reservation not found");
        }

        public async Task Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Guest>> GetGuests()
        {
            return await _context.Guests
                .Include(r => r.Reservations)
                .ToListAsync();
        }
    }
}
