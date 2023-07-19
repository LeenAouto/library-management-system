using Library.DAL.Abstractions;
using Library.Entities;

namespace Library.DAL.EF
{
    public class UserManager : IUserManager
    {
        private LibraryDbContext _context = new LibraryDbContext();

        public User Get(string username)
        {
            var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();
            return user;
        }

        public List<User> GetList()
        {
            List<User> users = new List<User>();
            List<User> allUsers = _context.Users.ToList();
            if (allUsers.Any())
            {
                foreach (User user in allUsers)
                {
                    users.Add(new User()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Password = user.Password,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Type = user.Type,
                        BirthDate = user.BirthDate,
                        Phone = user.Phone
                    });
                }
            }

            if (!(users.Any()))
            {
                return null;
            }
            return users;
        }
        public User Add(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return user;
            }
            catch
            {
                throw;
            }
        }
        public User Update(User user)
        {
            try
            {
                _context.Users.Update(user);

                _context.Entry(user).Property(p => p.Username).IsModified = false;
                _context.Entry(user).Property(p => p.Password).IsModified = false;

                _context.SaveChanges();
                return user;
            }
            catch
            {
                throw;
            }
        }

        public bool Delete(User user)
        {
            try
            {
                _context.Users.Remove(user);
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
