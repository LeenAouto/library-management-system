namespace Library.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public List<Reservation> reservations { get; set; }
    }
}
