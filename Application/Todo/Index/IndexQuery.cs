using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.Infrastructure;
using Application.Mediator;
using AsyncPoco;

namespace Application.Todo.Index
{
	public class IndexQuery : IQuery<Index>
	{
	}

	public class IndexQueryResult : TodoEntity
	{
		public string AssignedToUserName { get; set; }
		public string AssignedToDisplayName { get; set; }
	}

	public class IndexQueryHandler : IQueryHandler<IndexQuery, Index>
	{
		private const string TodoSql = @"
SELECT [Todo].*, UserName as [AssignedToUserName], DisplayName AS [AssignedToDisplayName] 
		FROM [Todo] 
			INNER JOIN [User] AS AssignedTo ON [AssignedTo].Id=AssignedToUserId 
		WHERE IsComplete=0 
		ORDER BY DisplayOrder DESC, CreatedOn";
		
		private const string UserSql = @"
SELECT UserName, DisplayName, COUNT(Todo.Id) AS TodoItemCount 
		FROM [User] LEFT JOIN [Todo] ON [User].Id = AssignedToUserId
		GROUP BY UserName, DisplayName";

		private readonly Database _database;

		public IndexQueryHandler(Database database)
		{
			_database = database;
		}

		public async Task<Index> RequestAsync(IndexQuery query)
		{
			var todo = await _database.FetchAsync<IndexQueryResult>(TodoSql);
			var items = todo.MapAll<TodoItem>().ToList();

			var users = await _database.FetchAsync<AssignedTo>(UserSql);
			
			return new Index
			{
				Items = items,
				Users = users.ToList()
			};
		}
	}
}