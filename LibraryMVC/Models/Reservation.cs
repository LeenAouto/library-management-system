using Microsoft.AspNetCore.Identity;

namespace LibraryMVC.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public Book? Book { get; set; }
        public int BookId { get; set; }
        public IdentityUser? IdentityUser { get; set; }
        public string IdentityUserId { get; set; }
        public string Username { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsReturned { get; set; }
        public Reservation()
        {

        }
    }
}
