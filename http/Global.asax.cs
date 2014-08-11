using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Application.Data;
using Application.Infrastructure;
using Application.Mediator;
using AsyncPoco;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Terabyte.Ioc;
using Terabyte.Mapper;
using Terademo.Mvc.Data;
using Terademo.Mvc.Data.Migrations;

namespace Terademo.Mvc
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			MigrationManager.ApplyMigrations();

			SetupIoc();
		}

		private static void SetupIoc()
		{
			Assembly webAssembly = typeof (MvcApplication).Assembly;
			Assembly applicationAssembly = typeof (TodoEntity).Assembly;

			var builder = new ContainerBuilder();
			builder.Register(_ => new Database("Application"))
				.InstancePerLifetimeScope();

			builder.RegisterDependency(new MappingDemandBuilder());
			builder.RegisterDependency(new AutoMapAllMappingProviders(webAssembly, applicationAssembly));
			builder.RegisterType<Mediator>().As<IMediator>();
			builder.RegisterAssemblyTypes(webAssembly, applicationAssembly)
				.AsClosedTypesOf(typeof (ICommandHandler<,>));
			builder.RegisterAssemblyTypes(webAssembly, applicationAssembly)
				.AsClosedTypesOf(typeof (IQueryHandler<,>));

			builder.RegisterControllers(webAssembly);
			IContainer container = builder.Build();

			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
			Mapping.SetMappingEngine(DependencyResolver.Current.GetService<IMappingEngine>());
		}
	}
}