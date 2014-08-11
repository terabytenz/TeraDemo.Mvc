using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.Infrastructure;
using Application.Mediator;
using AsyncPoco;

namespace Application.Todo.Create
{
	public class CreateTodoHandler : ICommandHandler<CreateTodo>
	{
		private readonly Database _database;

		public CreateTodoHandler(Database database)
		{
			_database = database;
		}

		public async Task<UnitType> SendAsync(CreateTodo command)
		{
			if (await _database.ExistsAsync<TodoEntity>("Title=@0 AND IsComplete=0", command.Title))
			{
				return UnitType.Default;
			}

			var userId = await _database.ExecuteScalarAsync<int?>("SELECT Id FROM [User] WHERE UserName=@0", command.AssignedTo);
			if (userId == null)
			{
				throw new ArgumentException("Unknown user \"" + command.AssignedTo + "\"");
			}

			var model = command.Map<TodoEntity>();
			model.AssignedToUserId = userId.Value;

			await _database.InsertAsync(model);

			return UnitType.Default;
		}
	}
}