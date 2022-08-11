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
    public class AssignNumberManager : IAssignNumberManager
    {
        private readonly ILogger<AssignNumberManager> _logger;
        private readonly TeamsPersistanceContext _dbContext;
        public AssignNumberManager(TeamsPersistanceContext dbContext, ILogger<AssignNumberManager> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<IQueryable<AssignNumber>> CreateAssignNumberAsync(string num)
        {
            var an = new AssignNumber();
            an.Num = num;
            an.IsUsed = 0;
            _dbContext.AssignNumbers.Add(an);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlUniqueConstraintViolationError)
            {
                _logger.LogError($"Create Number fail");
            }
            return _dbContext.AssignNumbers.Where(e => e.Num == num);
        }

        public IQueryable<AssignNumber> GetAssignNumbers() => _dbContext.AssignNumbers;

        public async Task<IQueryable<AssignNumber>> MarkNumberUserd(string num)
        {
            var an = await _dbContext.AssignNumbers.FirstOrDefaultAsync(n => n.Num == num);
            try
            {
                an.IsUsed = 1;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update Number fail");
            }
            return _dbContext.AssignNumbers.Where(n => n.Num == num);
        }
    }
}
