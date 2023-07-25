namespace Library.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int? PublishYear { get; set; }
        public bool IsAvailable { get; set; }

        public List<Reservation>? reservations { get; set; }
    }
}