using Common.Utilities;
using Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entitiesAssembly = typeof(IEntity).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(entitiesAssembly);
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddPluralizingTableNameConvention();
            modelBuilder.AddGuidDefaultValueSqlConvention();
             
            base.OnModelCreating(modelBuilder);
        }
    }
}
