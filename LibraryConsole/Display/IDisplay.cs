using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Entities;

namespace Library.ConsoleUI.Display
{
    public interface IDisplay
    {
        void DisplayBook(List<Book> books);
        void DisplayBook(Book book);
        void DisplayReservation(List<Reservation> reservations);

    }
}
