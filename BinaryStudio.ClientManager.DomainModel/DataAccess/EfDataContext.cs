using System.Data.Entity;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.DataAccess
{
    public class EfDataContext : DbContext
    {
        public EfDataContext()
#if DEBUG
            : base("ClientManager")
#endif
        {
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<MailMessage> MailMessages { get; set; }

        public DbSet<Inquiry> Inquiries { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
#if DEBUG
            Database.SetInitializer(new CreateDatabaseIfNotExists<EfDataContext>());
#endif
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<EfDataContext, EfMigrationConfiguration>());

            modelBuilder.Entity<Person>()
                .HasMany(x => x.RelatedMails)
                .WithMany(y => y.Receivers);
            modelBuilder.Entity<Inquiry>()
                .HasMany(x => x.Tags)
                .WithMany(y => y.Inquiries);
        }
    }
}