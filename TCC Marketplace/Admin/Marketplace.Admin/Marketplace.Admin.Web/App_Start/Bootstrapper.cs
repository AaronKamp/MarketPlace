using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Data.Repository;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Marketplace.Admin.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Autofac;
using Autofac.Integration.Mvc;
using Marketplace.Admin.Business;
using Microsoft.Owin.Security;

namespace Marketplace.Admin.App_Start
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            builder.RegisterType<UserStore>().As<IUserStore<IdentityUser, int>>().InstancePerRequest();
            builder.RegisterType<RoleStore>().InstancePerRequest();
            builder.RegisterType<SignInManager<IdentityUser, int>>().InstancePerRequest();
            builder.RegisterType<UserManager<IdentityUser, int>>().InstancePerRequest();


            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(ServiceRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            // Services
            builder.RegisterAssemblyTypes(typeof(ServiceManager).Assembly)
               .Where(t => t.Name.EndsWith("Manager"))
               .AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}