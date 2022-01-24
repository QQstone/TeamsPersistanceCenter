using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsPersistanceCenter.Models;

namespace TeamsPersistanceCenter.Managers.Interfaces
{
    public interface IAdministratorManager
    {
        IQueryable<Administrator> GetAdmins();
        IQueryable<Administrator> GetAdminByEmail(string email);
        Task<IQueryable<Administrator>> CreateAdminAsync(Administrator admin);
        Task<IQueryable<Administrator>> UpdateAdminAsync(Administrator admin);
        Task<IQueryable<Administrator>> DeactiveAdminAsync(string code);
    }
}
