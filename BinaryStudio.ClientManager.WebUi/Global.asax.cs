using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.DomainModel.Input;
using BinaryStudio.ClientManager.DomainModel.Tests.Input;
using OAuth2.Client;
using RestSharp;

namespace BinaryStudio.ClientManager.WebUi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Auth", // Route name
                            "Auth", // URL with parameters
                            new { controller = "Auth", action = "LogOn", id = UrlParameter.Optional });

            routes.MapRoute("Default", "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Inquiries", action = "Week", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            SetDependencyResolver();

            //TODO all works fine!
            mailMessageSaver = new MailMessageSaver(new EfRepository(), new AeEmailClient(TestAppConfiguration.GetTestConfiguration()));
        }

        private MailMessageSaver mailMessageSaver;

        private void SetDependencyResolver()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());

            builder
                .RegisterAssemblyTypes(
                    Assembly.GetAssembly(typeof (IIdentifiable)),
                    Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces();

            builder.RegisterType<EfRepository>().As<IRepository>().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly(), 
                    Assembly.GetAssembly(typeof(Client)), 
                    Assembly.GetAssembly(typeof(RestClient)))
                .AsImplementedInterfaces().AsSelf();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}