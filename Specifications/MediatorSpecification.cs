using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Mediator;
using Autofac;
using FluentAssertions;
using Machine.Specifications;

namespace Specifications
{
	public static class MediatorSpecification
	{
		[Subject(typeof (Mediator))]
		public class When_handler_does_not_exist
		{
			public static Mediator Subject;
			private static Exception exception;

			private Establish context = () => Subject = new Mediator(ContainerSetup.EmptyContainer());
			private Because of = () => AsyncCatch.Exception(() => Subject.RequestAsync(new Query()), out exception);

			private It should_be_invalid_operation = () => exception.Should().BeOfType<InvalidOperationException>();
			private It should_have_message = () => exception.Message.Should().MatchRegex("Query => QueryResult");
		}

		[Subject(typeof (Mediator))]
		public class When_have_single_handler
		{
			public static Mediator Subject;
			private static QueryResult result;

			private Establish context = () => Subject = new Mediator(ContainerSetup.Handler<QueryHandler>());
			private Because of = () => result = Subject.RequestAsync(new Query()).Await();

			private It should_not_be_null = () => result.Should().NotBeNull();
		}

		[Subject(typeof (Mediator))]
		public class When_have_anonymous_handler
		{
			public static Mediator Subject;

			private Establish context = () => Subject = new Mediator(ContainerSetup.AnonymousHandler());
			private Because of = () => Subject.RequestAsync(new Query()).Await();

			private It should_resolve_query = () => Subject.RequestAsync(new Query()).Await().Should().NotBeNull();
			private It should_resolve_command = () => Subject.SendAsync(new Command()).Await().Should().NotBeNull();
		}

		public class QueryHandler : IQueryHandler<Query, QueryResult>
		{
			public Task<QueryResult> RequestAsync(Query query)
			{
				return Task.FromResult(new QueryResult());
			}
		}

		public class Query : IQuery<QueryResult>
		{
		}

		public class QueryResult
		{
		}

		public class CommandHandler : ICommandHandler<Command, CommandResult>
		{
			public Task<CommandResult> SendAsync(Command command)
			{
				return Task.FromResult(new CommandResult());
			}
		}

		public class Command : ICommand<CommandResult>
		{
		}

		public class CommandResult
		{
		}
	}

	public static class ContainerSetup
	{
		public static IComponentContext EmptyContainer()
		{
			var builder = new ContainerBuilder();
			return builder.Build();
		}

		public static IComponentContext Handler<T>()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<T>().AsImplementedInterfaces();
			return builder.Build();			
		}

		public static IComponentContext AnonymousHandler()
		{
			var builder = new ContainerBuilder();
			builder.RegisterAssemblyTypes(typeof(MediatorSpecification).Assembly).AsImplementedInterfaces();
			return builder.Build();			
		}
	}
}