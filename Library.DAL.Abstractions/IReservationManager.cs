using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Abstractions
{
    public interface IReservationManager
    {
        Reservation Get(int? id);
        List<Reservation> GetList();
        Reservation Add(Reservation reservation);
        Reservation Update(Reservation reservation);
        bool Delete(Reservation reservation);
    }
}
