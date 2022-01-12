using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsPersistanceCenter.Models;

namespace TeamsPersistanceCenter.Managers.Interfaces
{
    public interface IUserManager
    {
        IQueryable<User> GetUsers();
        IQueryable<User> GetUserByCode(string code);
        Task<IQueryable<User>> CreateUserAsync(User user);
        Task<IQueryable<User>> UpdateUserAsync(User user);
        Task<IQueryable<User>> DeactiveUserAsync(string code);
    }
}
