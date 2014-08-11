using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.Infrastructure;
using Application.Mediator;
using AsyncPoco;

namespace Application.User.Create
{
	public class CreateUserHandler : ICommandHandler<CreateUser>
	{
		private readonly Database _database;

		public CreateUserHandler(Database database)
		{
			_database = database;
		}

		public async Task<UnitType> SendAsync(CreateUser command)
		{
			if (await _database.ExistsAsync<UserEntity>("UserName=@0", command.UserName))
			{
				return UnitType.Default;
			}

			var model = command.Map<UserEntity>();
			await _database.InsertAsync(model);

			return UnitType.Default;
		}
	}
}