using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data;
using Application.Infrastructure;
using Application.Todo.Details;
using FluentAssertions;
using Machine.Specifications;

namespace Specifications.Todo
{
	public static class DetailsQuerySpecifications
	{
		private static TestDatabase _db;

		private static void Setup()
		{
			_db = new TestDatabase().ApplyMigrations();
			Mapping.SetMappingEngine(MappingSetup.MappingEngine());

			_db.Database.InsertAsync(new TodoEntity { Title = "When_query_todo", AssignedToUserId = 1 }).Await();
		}

		private static TodoDetails Query(DetailsQuery query)
		{
			var handler = new DetailsQueryHandler(_db.Database);
			return handler.RequestAsync(query).Await();
		}

		[Subject(typeof(DetailsQueryHandler))]
		public class When_query_details
		{
			private static TodoDetails results;

			private Establish context = () => Setup();
			private Because of = () => results = Query(new DetailsQuery { Id = 1 });

			private It should_work = () => results.Should().NotBeNull();

			private It should_have_title = () => results.Title.Should().Be("When_query_todo");
			private It should_be_unassigned = () => results.AssignedTo.IsUnassignedUser.Should().BeTrue();

			private It should_have_user_list = () => results.Users.Should().NotBeEmpty();
			private It should_user_with_ticket = () => results.Users.Should().Contain(x => x.TodoItemCount > 0);

			private Cleanup cleanup = () => _db.Dispose();
		}

		[Subject(typeof(DetailsQueryHandler))]
		public class When_assigned_to_user
		{
			private static TodoDetails results;

			private Establish context = () =>
			{
				Setup();
				_db.Database.InsertAsync(new UserEntity { UserName = "user", DisplayName = "User" }).Await();
				_db.Database.UpdateAsync<TodoEntity>("SET AssignedToUserId=2").Await();
			};

			private Because of = () => results = Query(new DetailsQuery { Id = 1 });

			private It should_be_assigned = () => results.AssignedTo.IsUnassignedUser.Should().BeFalse();

			private Cleanup cleanup = () => _db.Dispose();
		}
	}
}