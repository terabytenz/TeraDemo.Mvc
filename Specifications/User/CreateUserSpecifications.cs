using System;
using System.Threading.Tasks;
using Application.Infrastructure;
using Application.User.Create;
using FluentAssertions;
using Machine.Specifications;

namespace Specifications.User
{
	public static class CreateUserSpecifications
	{
		private static TestDatabase _db;

		private static void Setup()
		{
			_db = new TestDatabase().ApplyMigrations();
			Mapping.SetMappingEngine(MappingSetup.MappingEngine());
		}

		private async static Task CreateUser(CreateUser item)
		{
			var handler = new CreateUserHandler(_db.Database);
			await handler.SendAsync(item);
		}

		[Subject(typeof(CreateUserHandler))]
		public class When_create_user
		{
			private Establish context = () => Setup();

			private Because of = () => CreateUser(new CreateUser { UserName = "user", DisplayName = "Username" }).Await();
			
			private It should_exist_in_database = () => _db.HaveUserAsync("user").Await().Should().BeTrue();

			private Cleanup cleanup = () => _db.Dispose();
		}

		[Subject(typeof (CreateUserHandler))]
		public class When_create_multiple_user
		{
			private Establish context = () => Setup();
			
			private Because of = () => Task.Run(async () =>
			{
				await CreateUser(new CreateUser { UserName = "user", DisplayName = "Username" });
				await CreateUser(new CreateUser { UserName = "user", DisplayName = "Username" });
			}).Await();

			private It should_not_create_duplicate = () => _db.CountUserAsync("user").Await().Should().Be(1);

			private Cleanup cleanup = () => _db.Dispose();
		}
	}
}