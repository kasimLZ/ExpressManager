namespace DataBase
{
    using Base;
    using Base.Model;
    using Base.Service;
    using Model;
    using System.Data.Entity;

    public class ApplicationDB : ApplicationBase
    {
        public ApplicationDB()
            : base("name=ApplicationDb")
        {
        }

        public DbSet<News> News { get; set; }


  
    }
}