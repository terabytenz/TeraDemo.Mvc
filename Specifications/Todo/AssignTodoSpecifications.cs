using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data;
using Application.Infrastructure;
using Application.Todo.Assign;
using Application.Todo.Complete;
using Application.Todo.Create;
using FluentAssertions;
using Machine.Specifications;

namespace Specifications.Todo
{
	public static class AssignTodoSpecifications
	{
		private static TestDatabase _db;

		private static void Setup()
		{
			_db = new TestDatabase().ApplyMigrations();
			Mapping.SetMappingEngine(MappingSetup.MappingEngine());

			_db.Database.InsertAsync(new TodoEntity {Title = "My Todo", AssignedToUserId = 1}).Await();
			_db.Database.InsertAsync(new UserEntity { UserName = "user", DisplayName = "User" }).Await();
		}

		private static void AssignTodo(AssignTodo item)
		{
			var handler = new AssignTodoHandler(_db.Database);
			handler.SendAsync(item).Await();
		}

		[Subject(typeof(AssignTodoHandler))]
		public class When_assign_todo
		{
			private Establish context = () => Setup();

			private Because of = () => AssignTodo(new AssignTodo() { Id = 1, AssignTo = "user"});

			private It should_be_assigned = () => _db.GetTodoAssigneeAsync(1).ContinueWith(task => task.Result.UserName == "user").Await().Should().BeTrue();

			private Cleanup cleanup = () => _db.Dispose();
		}
	}
}