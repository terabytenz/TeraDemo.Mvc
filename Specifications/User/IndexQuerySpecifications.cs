using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data;
using Application.Infrastructure;
using Application.User.Index;
using FluentAssertions;
using Machine.Specifications;

namespace Specifications.User
{
	public static class IndexQuerySpecifications
	{
		private static TestDatabase _db;

		private static void Setup()
		{
			_db = new TestDatabase().ApplyMigrations();
			Mapping.SetMappingEngine(MappingSetup.MappingEngine());

			_db.Database.InsertAsync(new UserEntity { UserName = "user", DisplayName = "User" }).Await();
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

			private It should_have_user_list = () => results.Users.Should().NotBeEmpty();
			private It should_have_single_result = () => results.Users.Select(x => x.DisplayName).Should().Contain("User");

			private Cleanup cleanup = () => _db.Dispose();
		}
	}
}