using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Abstractions
{
    public interface IBookManager
    {
        Book Get(int id);
        List<Book> GetList();
        Book Add(Book book);
        Book Update(Book book);
        bool Delete(Book book);
    }
}
