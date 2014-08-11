using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.Mediator;
using AsyncPoco;

namespace Application.Todo.Complete
{
	public class CompleteTodoHandler : ICommandHandler<CompleteTodo>
	{
		private readonly Database _database;

		public CompleteTodoHandler(Database database)
		{
			_database = database;
		}

		public async Task<UnitType> SendAsync(CompleteTodo command)
		{
			await _database.UpdateAsync<TodoEntity>("SET IsComplete=1 WHERE Id=@0", command.Id);
			return UnitType.Default;
		}
	}
}