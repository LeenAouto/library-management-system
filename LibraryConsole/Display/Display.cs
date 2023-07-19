using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Entities;

namespace Library.ConsoleUI.Display
{
    public class Display : IDisplay
    {
        public void DisplayBook(List<Book> books)
        {
            Console.WriteLine(String.Format("{0, -5} | {1, -25} | {2, -20} | {3, -20} | {4} | {5}",
                "ID", "Title", "Author", "Publisher", "Publication Year", "Is Available?"));
            string line = new String('-', 120);
            Console.WriteLine(line);

            foreach (Book book in books)
            {
                Console.WriteLine(String.Format("{0, -5} | {1, -25} | {2, -20} | {3, -20} | {4, -16} | {5}",
                        book.Id.ToString(),
                        book.Title.ToString(),
                        book.Author.ToString(),
                        book.Publisher.ToString(),
                        book.PublishYear.ToString(),
                        book.IsAvailable.ToString()));
            }
        }
        public void DisplayBook(Book book)
        {
            Console.WriteLine(String.Format("{0, -5} | {1, -25} | {2, -20} | {3, -20} | {4} | {5}",
                "ID", "Title", "Author", "Publisher", "Publication Year", "Is Available?"));
            string line = new String('-', 120);
            Console.WriteLine(line);

            Console.WriteLine(String.Format("{0, -5} | {1, -25} | {2, -20} | {3, -20} | {4, -16} | {5}",
                        book.Id.ToString(),
                        book.Title.ToString(),
                        book.Author.ToString(),
                        book.Publisher.ToString(),
                        book.PublishYear.ToString(),
                        book.IsAvailable.ToString()));
        }
        public void DisplayReservation(List<Reservation> reservations)
        {
            Console.WriteLine(String.Format("{0} | {1, -10} | {2, -10} | {3, -20} | {4, -20} | {5}",
                "ID", "Book ID", "User ID", "Username", "Start Date", "Is Returned?"));
            string line = new String('-', 90);
            Console.WriteLine(line);

            foreach (Reservation reservation in reservations)
            {
                Console.WriteLine(String.Format("{0, -2} | {1, -10} | {2, -10} | {3, -20} | {4, -20} | {5, -16}",
                    reservation.Id.ToString(),
                    reservation.UserId.ToString(),
                    reservation.BookId.ToString(),
                    reservation.Username.ToString(),
                    reservation.StartDate.ToString("yyyy-MM-dd"),
                    reservation.IsReturned.ToString()));
            }
        }
    }
}
