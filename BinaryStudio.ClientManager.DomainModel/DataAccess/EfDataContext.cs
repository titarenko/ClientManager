using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BinaryStudio.ClientManager.DomainModel.DataAccess
{
    public class EfDataContext : DbContext
    {
        public EfDataContext()
#if DEBUG
            : base("BinaryStudio.ClientManager.DomainModel.DataAccess.EfDataContext.Debug")
#endif
        {
            collections = new Dictionary<Type, object>
                {
                    {typeof (Story), Stories},
                    {typeof (Domain), Domains},
                    {typeof (User), Users}
                };
        }

        // classes Story, User, Domain should be implemented
                  
        public DbSet<Story> Stories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Domain> Domains { get; set; }

        public DbSet<T> GetDbSet<T>() where T : class
        {
            return collections[typeof(T)] as DbSet<T>;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            EfConnectionFactory.Enable(true);

#if DEBUG
            Database.SetInitializer(new CreateDatabaseIfNotExists<EfDataContext>());
#endif
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<EfDataContext, EfMigrationConfiguration>());
        }

        private readonly IDictionary<Type, object> collections;
    }
}