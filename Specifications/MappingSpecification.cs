using System;
using System.Collections;
using System.Linq;
using Application.Data;
using Application.Infrastructure;
using Application.Todo.Create;
using Application.Todo.Index;
using Autofac;
using AutoMapper;
using AutoMapper.Mappers;
using FluentAssertions;
using Machine.Specifications;
using Terabyte.Ioc;
using Terabyte.Mapper;
using Terademo.Mvc.Data;

namespace Specifications
{
	public static class MappingSpecifications
	{
		[Subject(typeof(Mapping))]
		public class When_no_mapper_registered
		{
			private static Exception exception;

			private Establish context = () => Mapping.SetMappingEngine(null);
			private Because of = () => exception = Catch.Exception(() => Enumerable.Empty<int>().MapAll<string>());
			private It should_throw_exception = () => exception.Should().NotBeNull();
		}

		[Subject(typeof(Mapping))]
		public class When_mapper_registered
		{
			private Establish context = () => Mapping.SetMappingEngine(MappingSetup.RawMappingEngine());
			private It can_map_all_items = () => Enumerable.Repeat(1, 3).MapAll<string>().Should().Equal(Enumerable.Repeat("1", 3));
			private It can_map_null_to_collection = () => ((IEnumerable ) null).MapAll<string>().Should().BeEmpty();
		}
	}

	public static class TodoMappings
	{
		[Subject(typeof(CreateTodoMappingProvider))]
		public class When_map_create_to_db
		{
			private static TodoEntity Subject;

			private Establish context = () => Mapping.SetMappingEngine(MappingSetup.MappingFor<CreateTodoMappingProvider>());
			private Because of = () => Subject = new CreateTodo { Title = "My Todo" }.Map<TodoEntity>();
			private It should_have_title = () => Subject.Title.Should().Be("My Todo");
			private It should_have_empty_description = () => Subject.Description.Should().BeEmpty();
			private It should_have_not_been_completed = () => Subject.IsComplete.Should().BeFalse();
		}

		[Subject(typeof(CreateTodoMappingProvider))]
		public class When_map_db_to_item
		{
			private static TodoItem Subject;

			private Establish context = () => Mapping.SetMappingEngine(MappingSetup.MappingFor<TodoItemMappingProvider>());
			private Because of = () => Subject = new IndexQueryResult { Id = 1, Title = "My Todo" }.Map<TodoItem>();
			private It should_have_id = () => Subject.Id.Should().Be(1);
			private It should_have_title = () => Subject.Title.Should().Be("My Todo");
		}
	}

	public static class MappingSetup
	{
		public static IMappingEngine RawMappingEngine()
		{
			return new MappingEngine(new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers()));
		}

		public static IMappingEngine MappingEngine()
		{
			var builder = new ContainerBuilder();
			builder.RegisterDependency(new MappingDemandBuilder());
			builder.RegisterDependency(new AutoMapAllMappingProviders(typeof (TodoEntity).Assembly));
			
			var container = builder.Build();
			return container.Resolve<IMappingEngine>();
		}

		public static IMappingEngine MappingFor<T>() where T : IMappingProvider
		{
			var builder = new ContainerBuilder();
			builder.RegisterDependency(new MappingDemandBuilder());
			builder.RegisterType<T>().As<IMappingProvider>();
			var container = builder.Build();

			return container.Resolve<IMappingEngine>();
		}
	}
}