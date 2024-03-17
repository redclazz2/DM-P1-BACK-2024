using api.Data;
using api.Dto;
using api.Interfaces;
using api.Models;

namespace api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext _context){
            this._context = _context;
        }
        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return Save();
        }

        public ICollection<User> GetUser(UserDto user)
        {
            return _context.Users.Where(u => u.Email == user.Email).ToList();
        }

        public bool Save()
        {
            var _saved = _context.SaveChanges();
            return _saved > 0 ? true : false;
        }
    }
}