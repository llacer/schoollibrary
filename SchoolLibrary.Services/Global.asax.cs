using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using SchoolLibrary.Common.Helpers;
using SchoolLibrary.DataAccess.Entity;
using SchoolLibrary.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SchoolLibrary.Services
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitialiseIoc();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void InitialiseIoc()
        {
            ContainerBuilder Builder = new ContainerBuilder();
            Builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            Builder.Register(ctx =>
            {
                var filePath = HostingEnvironment.MapPath("~/App_Data") + ConfigHelper.BooksFilePath;
                return new XMLRepository<Book>(filePath);
            }).As<IRepository<Book>>();

            Builder.Register(ctx =>
            {
                var filePath = HostingEnvironment.MapPath("~/App_Data") + ConfigHelper.MessagesFilePath;
                return new XMLRepository<Message>(filePath);
            }).As<IRepository<Message>>();

            IContainer Container = Builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }
    }
}
