using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Pluralize.NET;
using System.Reflection;

namespace Common.Utilities
{
    public static class ModelBuilderExtensions
    {
        public static void RegisterAllEntities<TEntity>(this ModelBuilder modelBuilder,params Assembly[] assemblies)
        {
            assemblies
                .SelectMany(a=>a.GetExportedTypes())
                .Where(t=>t.IsClass && t.IsPublic && !t.IsAbstract && t.IsAssignableFrom(typeof(TEntity)))
                .ToList()
                .ForEach(t=>modelBuilder.Entity(t));
        }
        public static void AddRestrictDeleteBehaviorConvention(this ModelBuilder modelBuilder)
        {
            modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                .ToList()
                .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict); ;
        }

        public static void AddDefaultValueSqlConvention(this ModelBuilder modelBuilder,Type propertyType, string propertyName,string defaultValueSql) 
        {
            IEnumerable<IMutableProperty> properties = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == propertyType && p.Name == propertyName);
            foreach(IMutableProperty property in properties)
                if(property!=null)
                    property.SetDefaultValueSql(defaultValueSql);
        }

        public static void AddGuidDefaultValueSqlConvention(this ModelBuilder modelBuilder)
        {
            AddDefaultValueSqlConvention(modelBuilder, typeof(Guid), "Id", "NEWSEQUENTIALID()");
        }

        public static void AddPluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            var pluralizer = new Pluralizer();
            foreach (var item in modelBuilder.Model.GetEntityTypes())
                item.SetTableName(pluralizer.Pluralize(item.GetTableName()));
        }

        public static void AddSingularizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            var pluralizer = new Pluralizer();
            foreach (var item in modelBuilder.Model.GetEntityTypes())
                item.SetTableName(pluralizer.Singularize(item.GetTableName()));
        }
    }
}
