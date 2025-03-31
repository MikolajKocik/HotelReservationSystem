using HotelReservationSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Controllers
{
    public class GuestController : Controller
    {
        private readonly HotelDbContext _context;

        public GuestController(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var guests = await _context.Guests.ToListAsync();
            return View(guests);
        }
    }
}
