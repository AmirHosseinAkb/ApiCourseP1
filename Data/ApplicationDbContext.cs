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

        private void _cleanString()
        {
            var changedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var entry in changedEntries)
            {
                var properties = entry.GetType()
                    .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    .Where(p => p.PropertyType == typeof(string));
                foreach(var property in properties)
                {
                    var propValue =(string)property.GetValue(entry.Entity, null);
                    if (propValue.HasValue())
                    {
                        var newValue = propValue.Fa2En().FixPersianChars();
                        property.SetValue(entry.Entity, newValue, null);
                    }
                }
            }
        }

        public override int SaveChanges()
        {
            _cleanString();
            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
