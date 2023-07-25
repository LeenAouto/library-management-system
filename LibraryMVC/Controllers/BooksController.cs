using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryMVC.Data;
using LibraryMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Library.DAL.EF;
using Library.DAL.Abstractions;

namespace LibraryMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookManager bookManager;

        public BooksController(ApplicationDbContext context, IBookManager bookManager)
        {
            _context = context;
            this.bookManager = bookManager;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
              return bookManager.GetList != null ? 
                          View(bookManager.GetList()) :
                          Problem("Entity set 'ApplicationDbContext.Book'  is null.");
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || bookManager.GetList() == null)
            {
                return NotFound();
            }

            var book = bookManager.Get(id);
                
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Publisher,PublishYear,IsAvailable")] Library.Entities.Book book)
        {
            if (ModelState.IsValid)
            {
                bookManager.Add(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || bookManager.GetList() == null)
            {
                return NotFound();
            }

            var book = bookManager.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Publisher,PublishYear,IsAvailable")] Library.Entities.Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bookManager.Update(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || bookManager.GetList() == null)
            {
                return NotFound();
            }

            var book = bookManager.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (bookManager.GetList() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Book'  is null.");
            }
            var book = bookManager.Get(id);
            if (book != null)
            {
                bookManager.Delete(book);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return bookManager.Get(id) != null ? true : false;
            //return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
