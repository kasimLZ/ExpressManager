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
            AutomaticMigrationsEnabled = true;//ʵ�������
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DataBase.ApplicationDB context)
        {
            
        }
        
    }
}
