using System;
using System.Threading.Tasks;
using Application.Infrastructure;
using Application.Todo.Create;
using FluentAssertions;
using Machine.Specifications;

namespace Specifications.Todo
{
	public static class CreateTodoSpecifications
	{
		private static TestDatabase _db;

		private static void Setup()
		{
			_db = new TestDatabase().ApplyMigrations();
			Mapping.SetMappingEngine(MappingSetup.MappingEngine());
		}

		private static async Task CreateTodo(CreateTodo item)
		{
			var handler = new CreateTodoHandler(_db.Database);
			await handler.SendAsync(item);
		}

		[Subject(typeof(CreateTodoHandler))]
		public class When_create_todo
		{
			private Establish context = () => Setup();

			private Because of = () => CreateTodo(new CreateTodo { Title = "My todo", AssignedTo = "" }).Await();
			
			private It should_exist_in_database = () => _db.HaveTodoAsync("My todo").Await().Should().BeTrue();

			private Cleanup cleanup = () => _db.Dispose();
		}

		[Subject(typeof(CreateTodoHandler))]
		public class When_create_todo_with_bad_user
		{
			private static Exception exception;

			private Establish context = () => Setup();

			private Because of = () => AsyncCatch.Exception(() => CreateTodo(new CreateTodo { Title = "My todo", AssignedTo = "does-not-exist" }), out exception);
			
			private It should_throw_argument_exception = () => exception.Should().BeOfType<ArgumentException>();
			private It should_have_message = () => exception.Message.Should().MatchRegex("Unknown user");

			private Cleanup cleanup = () => _db.Dispose();
		}

		[Subject(typeof (CreateTodoHandler))]
		public class When_create_multiple_todo
		{
			private Establish context = () => Setup();

			private Because of = () => Task.Run(async () =>
			{
				await CreateTodo(new CreateTodo {Title = "My todo", AssignedTo = ""});
				await CreateTodo(new CreateTodo {Title = "My todo", AssignedTo = ""});
			}).Await();

			private It should_not_create_duplicate = () => _db.CountTodoAsync("My todo").Await().Should().Be(1);

			private Cleanup cleanup = () => _db.Dispose();
		}
	}
}