using System.ComponentModel.DataAnnotations;

namespace Library.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [RegularExpression(@"^0+5+\d{8}$", ErrorMessage = "Invalid Phone Number.")]
        public string Phone { get; set; }
        public List<Reservation>? reservations { get; set; }
    }
}
