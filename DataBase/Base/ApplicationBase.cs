namespace DataBase.Base
{
	using DataBase.Base.Heplers;
	using Model;
    using Service;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Reflection;

	public class ApplicationBase : SysApplicationDb
	{
		public ApplicationBase()
			: base("DefaultConnection")
		{
		}

		public ApplicationBase(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}


		public DbSet<SysArea> SysAreas { get; set; }
		public DbSet<SysAction> SysActions { get; set; }
		public DbSet<SysController> SysControllers { get; set; }
		public DbSet<SysControllerSysAction> SysControllerSysActions { get; set; }
		public DbSet<SysRole> SysRoles { get; set; }
		public DbSet<SysRoleSysControllerSysAction> SysRoleSysControllerSysAction { get; set; }
		public DbSet<SysRoleSysUserInfo> SysRoleSysUserInfo { get; set; }
		public DbSet<SysUserInfo> SysUserInfos { get; set; }
		public DbSet<SysErrorLog> SysErrorLogs { get; set; }
		public DbSet<UserDict> UserDicts { get; set; }
		public DbSet<UserDictType> UserDictTypes { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Conventions.Add<DecimalPrecisionAttributeConvention>();
			modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
