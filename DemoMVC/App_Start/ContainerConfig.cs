using Autofac;
using Autofac.Integration.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace DemoMVC
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            var domains = new List<string>() { ".Repositories", ".Services", ".DataAccess" };
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => domains.Any(d => x.Namespace.EndsWith(d)))
                .AsImplementedInterfaces();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}