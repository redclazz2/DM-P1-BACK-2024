using api.Dto;
using api.Models;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUser(UserDto user);
        bool CreateUser(User user);
        bool Save();     
    }
}