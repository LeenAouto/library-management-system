namespace Library.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsReturned { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }

    }
}
