using InternalBookingApp.Data;
using InternalBookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using InternalBookingApp.Models.Entities;
namespace InternalBookingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Dashboard()
{
    var today = DateTime.Today;
    var upcomingBookings = await _context.Bookings
        .Include(b => b.Resource)
        .Where(b => b.StartTime.Date == today)
        .OrderBy(b => b.StartTime)
        .ToListAsync();

    return View(upcomingBookings);
}

    }
}
