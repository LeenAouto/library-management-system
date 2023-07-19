using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Abstractions
{
    public interface IUserManager
    {
        User Get(string username);
        List<User> GetList();
        User Add(User user);
        User Update(User user);
        bool Delete(User user);
    }
}
