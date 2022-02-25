using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsPersistanceCenter.Models;

namespace TeamsPersistanceCenter.Managers.Interfaces
{
    public interface IAssignNumberManager
    {
        IQueryable<AssignNumber> GetAssignNumbers();
        Task<IQueryable<AssignNumber>> CreateAssignNumberAsync(String num);
        Task<IQueryable<AssignNumber>> MarkNumberUserd(String num);
    }
}
