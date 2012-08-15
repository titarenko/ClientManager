using System;
using System.Reflection;
using System.Security.Principal;
using System.Web;
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
using log4net;

namespace BinaryStudio.ClientManager.WebUi
{
    public class MvcApplication : HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));

        private static MailMessagePersister mailMessagePersister;

        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute("Auth", // Route name
            //                "Auth", // URL with parameters
            //                new { controller = "Auth", action = "LogOn", id = UrlParameter.Optional });

            routes.MapRoute("Default", 
                            "{controller}/{action}/{id}", // URL with parameters
                            new { controller = "Inquiries", action = "Week", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();

            log.Error("App_Error", ex);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var currentUser = DependencyResolver.Current.GetService<IAppContext>().User;
            HttpContext.Current.User =
                currentUser == null ? null : new GenericPrincipal(new GenericIdentity(currentUser.RelatedPerson.Email), null);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            SetDependencyResolver();

            if (null == mailMessagePersister)
            {
                //mailMessagePersister = DependencyResolver.Current.GetService<MailMessagePersister>();
                //TODO delete
                mailMessagePersister = new MailMessagePersister(new EfRepository(),
                                                                new AeEmailClient(TestAppConfiguration.GetTestConfiguration()),
                                                                new InquiryFactory(),
                                                                new MailMessageParserFactory());
            }

            log4net.Config.XmlConfigurator.Configure();
            //LogManager.GetLogger(typeof(AppConfiguration)).Fatal("We are the champion");
        }

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

            builder.RegisterType<InquiryFactory>().As<IInquiryFactory>();

            builder.RegisterType<MailMessageParserFactory>().As<IMailMessageParserFactory>();

            builder.RegisterType<AppContext>().As<IAppContext>();

            builder.RegisterType<EfRepository>().As<IRepository>().InstancePerHttpRequest();
            //builder.Register(c => new MultitenantRepository(new EfRepository(), c.Resolve<IAppContext>())).
            //    As<IRepository>().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly(), 
                    Assembly.GetAssembly(typeof(Client)), 
                    Assembly.GetAssembly(typeof(RestClient)))
                .AsImplementedInterfaces().AsSelf();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}