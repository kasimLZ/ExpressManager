namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public class Configuration : DbMigrationsConfiguration<ApplicationDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;//实体类更新
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DataBase.ApplicationDB context)
        {
            
        }
        
    }
}
