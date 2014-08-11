using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data;
using Application.Infrastructure;
using Application.Todo.Complete;
using Application.Todo.Create;
using FluentAssertions;
using Machine.Specifications;
using Terademo.Mvc.Data;

namespace Specifications.Todo
{
	public static class CompleteTodoSpecifications
	{
		private static TestDatabase _db;

		private static void Setup()
		{
			_db = new TestDatabase().ApplyMigrations();
			Mapping.SetMappingEngine(MappingSetup.MappingEngine());

			_db.Database.InsertAsync(new TodoEntity {Title = "My Todo", AssignedToUserId = 1}).Await();
		}

		private static void CompleteTodo(CompleteTodo item)
		{
			var handler = new CompleteTodoHandler(_db.Database);
			handler.SendAsync(item).Await();
		}

		[Subject(typeof(CompleteTodoHandler))]
		public class When_create_todo
		{
			private Establish context = () => Setup();

			private Because of = () => CompleteTodo(new CompleteTodo() { Id = 1 });

			private It should_be_flagged_complete_in_database = () => _db.IsTodoCompleteAsync(1).Await().Should().BeTrue();

			private Cleanup cleanup = () => _db.Dispose();
		}
	}
}