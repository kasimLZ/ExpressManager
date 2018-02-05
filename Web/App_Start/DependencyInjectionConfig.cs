using Autofac;
using Autofac.Integration.Mvc;
using System.Linq;
using DataBase;
using DataBase.Base.Interface;
using DataBase.Base.Service.Infrastructure;
using DataBase.Base.Infrastructure.Interface;
using System.Reflection;
using Web.Helper;
using Common;

namespace Web.App_Start
{
    public class DependencyInjectionConfig
    {
        public static AutofacDependencyResolver Register()
        {
            var builder = new ContainerBuilder();

            //数据库上下文
            builder.RegisterType<ApplicationDB>().AsImplementedInterfaces();

            //用户登陆信息，初始化信息的方法可以在登陆控制器中扩展
            builder.RegisterType<CurrentUser>().As<ICurrentUser>();

            //注册工厂类
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            //泛型仓储基类注册，以此避免直接暴露上下文
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>));

            //批量注册自定义服务
            builder.RegisterAssemblyTypes(Assembly.Load("Database"))
                .Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();


            //注册当前项目下所有控制器
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            return new AutofacDependencyResolver(builder.Build());
        }
    }
}