using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsPersistanceCenter.Managers.Interfaces;
using TeamsPersistanceCenter.Models;
using TeamsPersistanceCenter.Models.BaseModels;
using TeamsPersistanceCenter.Models.Contexts;

namespace TeamsPersistanceCenter.Managers.Managers
{
    public class UserManager : IUserManager
    {
        private readonly ILogger<UserManager> _logger;
        private readonly TeamsPersistanceContext _dbContext;
        public UserManager(TeamsPersistanceContext dbContext, ILogger<UserManager> logger){
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<IQueryable<User>> CreateUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlUniqueConstraintViolationError)
            {
                _logger.LogError($"Create User fail");
            }
            return _dbContext.Users.Where(e => e.Code == user.Code);
        }

        public async Task<IQueryable<User>> DeactiveUserAsync(string code)
        {
            try
            {
                var user = await GetUserByCode(code).FirstOrDefaultAsync();
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete User fail", ex);
            }
            return GetUsers();
        }

        public IQueryable<User> GetUserByCode(string code)
        {
            return GetUsers().Where(user => user.Code == code);

        }
        public IQueryable<User> GetUsers() => _dbContext.Users;


        public async Task<IQueryable<User>> UpdateUserAsync(User user)
        {
            var ExitingUser = await _dbContext.Users.Where(u => u.Code == user.Code).AsNoTracking().FirstOrDefaultAsync();
            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update User fail", ex);
            }
            return _dbContext.Users.Where(u => u.Code == user.Code);
        }
    }
}
