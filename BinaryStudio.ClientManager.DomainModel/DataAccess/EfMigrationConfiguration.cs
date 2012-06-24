using System.Data.Entity.Migrations;

namespace BinaryStudio.ClientManager.DomainModel.DataAccess
{
    public class EfMigrationConfiguration : DbMigrationsConfiguration<EfDataContext>
    {
        public EfMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
#if DEBUG
            AutomaticMigrationDataLossAllowed = true;
#endif
        }
    }
}