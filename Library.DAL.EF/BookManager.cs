using Library.DAL.Abstractions;
using Library.Entities;

namespace Library.DAL.EF
{
    public class BookManager : IBookManager
    {
        private LibraryDbContext _context = new LibraryDbContext();

        public Book Get(int id)
        {
            var book = _context.Books.Where(x => x.Id == id).FirstOrDefault();
            return book;
        }

        public List<Book> GetList()
        {
            List<Book> books = _context.Books.ToList();
            if (!(books.Any()))
            {
                return null;
            }
            return books;
        }
        public Book Add(Book book)
        {
            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                return book;
            }
            catch
            {
                throw;
            }
        }
        public Book Update(Book book)
        {
            try
            {
                _context.Books.Update(book);
                _context.SaveChanges();
                return book;
            }
            catch
            {
                throw;
            }
        }
        public bool Delete(Book book)
        {
            try
            {
                _context.Books.Remove(book);
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
