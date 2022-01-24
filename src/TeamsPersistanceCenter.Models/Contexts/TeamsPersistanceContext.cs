using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TeamsPersistanceCenter.Models.BaseModels;
using TeamsPersistanceCenter.Models.Enums;

namespace TeamsPersistanceCenter.Models.Contexts
{
    public class TeamsPersistanceContext : DbContext
    {
        public TeamsPersistanceContext(DbContextOptions<TeamsPersistanceContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Administrator> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaving();
            try
            {
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                // Is this an unique constraint violation error?
                if (sqlEx.Number == Convert.ToInt32(SqlDbErrorCode.UniqueConstraintViolation))
                    throw new SqlUniqueConstraintViolationError(sqlEx.Message, sqlEx);
                throw sqlEx;
            }
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            BeforeSaving();
            try
            {
                var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                return result;
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                // Is this an unique constraint violation error?
                if (sqlEx.Number == Convert.ToInt32(SqlDbErrorCode.UniqueConstraintViolation))
                    throw new SqlUniqueConstraintViolationError(sqlEx.Message, sqlEx);
                throw sqlEx;
            }
        }

        private void BeforeSaving()
        {
            //var entries = ChangeTracker
            //    .Entries()
            //    .Where(e => e.Entity is IDbEntityBase && (e.State == EntityState.Modified));

            //foreach (var entityEntry in entries)
            //{
            //    ((IDbEntityBase)entityEntry.Entity).RecordLastUpdated = DateTime.UtcNow;
            //}
        }
    }
}
