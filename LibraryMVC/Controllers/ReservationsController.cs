using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Library.DAL.Abstractions;
using Library.DAL.EF;
using Library.Entities;

namespace LibraryMVC.Controllers
{
	[Authorize(Policy = "StaffPolicy")]
	public class ReservationsController : Controller
    {
		private readonly IReservationManager reservationManager;

		public ReservationsController(IReservationManager reservationManager)
        {
			this.reservationManager = reservationManager;
		}

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
			return reservationManager.GetList != null ?
						  View(reservationManager.GetList()) :
						  Problem("Entity set 'LibraryDbContext.Reservations'  is null.");
		}

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || reservationManager.GetList == null)
            {
                return NotFound();
			}

			var reservation = reservationManager.Get(id);

			if (reservation == null)
			{
				return NotFound();
			}

			return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            //ViewData["BookId"] = new SelectList(_context.Book, "Id", "Id");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,UserId,Username,StartDate,IsReturned")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservationManager.Add(reservation);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["BookId"] = new SelectList(_context.Book, "Id", "Id", reservation.BookId);
            //ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", reservation.IdentityUserId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
			if (id == null || reservationManager.GetList() == null)
			{
				return NotFound();
			}

			var reservation = reservationManager.Get(id);
			if (reservation == null)
			{
				return NotFound();
			}
			//ViewData["BookId"] = new SelectList(_context.Book, "Id", "Id", reservation.BookId);
			//ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", reservation.IdentityUserId);
			return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,UserId,Username,StartDate,IsReturned")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

			if (ModelState.IsValid)
			{
				try
				{
					reservationManager.Update(reservation);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ReservationExists(reservation.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
            //ViewData["BookId"] = new SelectList(_context.Book, "Id", "Id", reservation.BookId);
            //ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", reservation.IdentityUserId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    var reservation = await _context.Reservation
        //        .Include(r => r.Book)
        //        .Include(r => r.IdentityUser)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(reservation);
        //}

        //// POST: Reservations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Reservation == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
        //    }
        //    var reservation = await _context.Reservation.FindAsync(id);
        //    if (reservation != null)
        //    {
        //        _context.Reservation.Remove(reservation);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ReservationExists(int id)
        {
			return reservationManager.Get(id) != null ? true : false;
			//return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
		}
    }
}
