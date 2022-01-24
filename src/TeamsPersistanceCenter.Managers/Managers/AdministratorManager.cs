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
    public class AdministratorManager : IAdministratorManager
    {
        private readonly ILogger<AdministratorManager> _logger;
        private readonly TeamsPersistanceContext _dbContext;
        public async Task<IQueryable<Administrator>> CreateAdminAsync(Administrator administrator)
        {
            _dbContext.Admins.Add(administrator);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlUniqueConstraintViolationError)
            {
                _logger.LogError($"Create Administrator fail");
            }
            return _dbContext.Admins.Where(e => e.Email == administrator.Email);
        }

        public async Task<IQueryable<Administrator>> DeactiveAdminAsync(string Id)
        {
            try
            {
                var administrator = await GetAdminByEmail(Id).FirstOrDefaultAsync();
                _dbContext.Admins.Remove(administrator);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete Administrator fail");
            }
            return GetAdmins();
        }

        public IQueryable<Administrator> GetAdminByEmail(string email)
        {
            return GetAdmins().Where(administrator => administrator.Email == email);

        }
        public IQueryable<Administrator> GetAdmins() => _dbContext.Admins;


        public async Task<IQueryable<Administrator>> UpdateAdminAsync(Administrator administrator)
        {
            var ExitingAdmin = await _dbContext.Admins.FirstOrDefaultAsync(admin=>admin.Id== administrator.Id);
            try
            {
                ExitingAdmin.Email = administrator.Email;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update Administrator fail");
            }
            return _dbContext.Admins.Where(admin => admin.Id == administrator.Id);
        }
    }
}
