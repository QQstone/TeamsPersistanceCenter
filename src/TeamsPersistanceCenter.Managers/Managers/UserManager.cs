using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsPersistanceCenter.Managers.Interfaces;
using TeamsPersistanceCenter.Models;

namespace TeamsPersistanceCenter.Managers.Managers
{
    public class UserManager : IUserManager
    {
        public Task<IQueryable<User>> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<User>> DeactiveUserAsync(string code)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetUserByCode(string code)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<User>> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
