using InternalBookingApp.Data;
using InternalBookingApp.Models;
using InternalBookingApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalBookingApp.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
           this._context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(int? resourceId, DateTime? date)
        {
            // Populate dropdown
            ViewBag.Resources = new SelectList(_context.Resources, "Id", "Name");

            var bookingsQuery = _context.Bookings
                .Include(b => b.Resource)
                .AsQueryable();

            if (resourceId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.Id == resourceId.Value);
            }

            if (date.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b =>
                    b.StartTime.Date == date.Value.Date ||
                    b.EndTime.Date == date.Value.Date
                );
            }

            var bookings = await bookingsQuery.OrderBy(b => b.StartTime).ToListAsync();
            return View(bookings);
        }


        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewBag.ResourceId = new SelectList(_context.Resources, "Id", "Name");
            return View();
        }


        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking
                {
                    ResourceId = model.ResourceId,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    BookedBy = model.BookedBy,
                    Purpose = model.Purpose
                };

                // ? Validate: EndTime must be after StartTime
                        if (booking.EndTime <= booking.StartTime)
                        {
                            ModelState.AddModelError("", "End time must be after start time.");
                           ViewBag.ResourceId = new SelectList(_context.Resources, "Id", "Name", booking.ResourceId);
                           return View(booking);
                        }

                        // ? Validate: Booking conflict detection
                        bool conflict = _context.Bookings.Any(b =>
                            b.ResourceId == booking.ResourceId &&
                            ((booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                             (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
                             (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime))
                        );

                       if (conflict)
                      {
                            ModelState.AddModelError("", "This resource is already booked during the selected time.");
                            ViewBag.ResourceId = new SelectList(_context.Resources, "Id", "Name", booking.ResourceId);
                            return View(booking);
                        }

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

          
            return View(model);
        }




        // GET: Bookings/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Id", booking.ResourceId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Booking viewModel)
        {
            var booking = await _context.Bookings.FindAsync(viewModel.Id);

            if (booking is not null)
            {
                booking.StartTime = viewModel.StartTime;
                booking.EndTime = viewModel.EndTime;
                booking.BookedBy = viewModel.BookedBy;
                booking.Purpose = viewModel.Purpose;              
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));


        }

     

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }


        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
