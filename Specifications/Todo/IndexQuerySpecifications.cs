using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data;
using Application.Infrastructure;
using Application.Todo.Index;
using FluentAssertions;
using Machine.Specifications;

namespace Specifications.Todo
{
	public static class IndexQuerySpecifications
	{
		private static TestDatabase _db;

		private static void Setup()
		{
			_db = new TestDatabase().ApplyMigrations();
			Mapping.SetMappingEngine(MappingSetup.MappingEngine());

			_db.Database.InsertAsync(new TodoEntity { Title = "When_query_todo", AssignedToUserId = 1 }).Await();
		}

		private static Index Query(IndexQuery query)
		{
			var handler = new IndexQueryHandler(_db.Database);
			return handler.RequestAsync(query).Await();
		}

		[Subject(typeof(IndexQueryHandler))]
		public class When_query_todo
		{
			private static Index results;

			private Establish context = () => Setup();
			private Because of = () => results = Query(new IndexQuery());

			private It should_have_single_result = () => results.Items.Select(x => x.Title).Should().Contain("When_query_todo");
			private It should_have_item_with_assigned_user = () => results.Items.Select(x => x.AssignedTo).Single().Should().NotBeNull();
			private It should_have_user_list = () => results.Users.Should().NotBeEmpty();
			private It should_user_with_ticket = () => results.Users.Should().OnlyContain(x => x.TodoItemCount > 0);

			private Cleanup cleanup = () => _db.Dispose();
		}

		[Subject(typeof(IndexQueryHandler))]
		public class When_user_has_no_items
		{
			private static Index results;

			private Establish context = () =>
			{
				Setup();
				_db.Database.InsertAsync(new UserEntity { UserName = "user", DisplayName = "User" }).Await();

			};
			private Because of = () => results = Query(new IndexQuery());

			private It should_have_user_without_ticket = () => results.Users.Should().Contain(x => x.TodoItemCount == 0);

			private Cleanup cleanup = () => _db.Dispose();
		}
	}
}