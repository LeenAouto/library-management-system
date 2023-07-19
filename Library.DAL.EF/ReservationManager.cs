﻿using Library.DAL.Abstractions;
using Library.Entities;

namespace Library.DAL.EF
{
    public class ReservationManager : IReservationManager
    {
        private LibraryDbContext _context = new LibraryDbContext();

        public Reservation Get(int id)
        {
            var reservation = _context.Reservations.Where(x => x.Id == id).FirstOrDefault();
            return reservation;
        }
        public List<Reservation> GetList()
        {
            List<Reservation> reservations = new List<Reservation>();
            List<Reservation> allReservations = _context.Reservations.ToList();
            if (allReservations.Any())
            {
                foreach (Reservation reservation in allReservations)
                {
                    reservations.Add(new Reservation()
                    {
                        Id = reservation.Id,
                        BookId = reservation.BookId,
                        UserId = reservation.UserId,
                        Username = reservation.Username,
                        StartDate = reservation.StartDate,
                        IsReturned = reservation.IsReturned,

                    });
                }
            }

            if (!(reservations.Any()))
            {
                return null;
            }
            return reservations;
        }
        public Reservation Add(Reservation reservation)
        {
            try
            {
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                return reservation;
            }
            catch
            {
                throw;
            }
        }
        public Reservation Update(Reservation reservation)
        {
            try
            {
                _context.Reservations.Update(reservation);
                _context.Entry(reservation).Property(p => p.UserId).IsModified = false;
                _context.Entry(reservation).Property(p => p.Username).IsModified = false;
                _context.SaveChanges();
                return reservation;
            }
            catch
            {
                throw;
            }
        }
        public bool Delete(Reservation reservation)
        {
            try
            {
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
