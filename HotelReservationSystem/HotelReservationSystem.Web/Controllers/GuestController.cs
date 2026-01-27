using HotelReservationSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Controllers
{
    public class GuestController : Controller
    {
        private readonly HotelDbContext context;

        public GuestController(HotelDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> List()
        {
            var guests = await context.Guests.ToListAsync();
            return View(guests);
        }
    }
}
