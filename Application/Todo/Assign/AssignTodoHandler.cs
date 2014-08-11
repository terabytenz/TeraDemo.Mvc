using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.Mediator;
using AsyncPoco;

namespace Application.Todo.Assign
{
	public class AssignTodoHandler : ICommandHandler<AssignTodo>
	{
		private readonly Database _database;

		public AssignTodoHandler(Database database)
		{
			_database = database;
		}

		public async Task<UnitType> SendAsync(AssignTodo command)
		{
			await _database.UpdateAsync<TodoEntity>("SET AssignedToUserId=(SELECT Id From [User] WHERE UserName=@1) WHERE Id=@0", command.Id, command.AssignTo);
			return UnitType.Default;
		}
	}
}