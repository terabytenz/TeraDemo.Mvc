using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.Infrastructure;
using Application.Mediator;
using Application.Todo.Index;
using AsyncPoco;
using AutoMapper;
using Terabyte.Mapper;

namespace Application.Todo.Details
{
	public class DetailsQuery : IQuery<TodoDetails>
	{
		public int Id { get; set; } 
	}

	public class TodoDetails
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public AssignedTo AssignedTo { get; set; }
		public IEnumerable<AssignedTo> Users { get; set; }
	}

	public class TodoDetailsMappingProvider : IMappingProvider
	{
		public void RegisterTypeMappings(IConfiguration config)
		{
			config.CreateMap<TodoEntity, TodoDetails>();
			config.CreateMap<UserEntity, TodoDetails>()
				.ForMember(x => x.AssignedTo, map => map.ResolveUsing(x => new AssignedTo { UserName = x.UserName, DisplayName = x.DisplayName }));
		}
	}

	public class DetailsQueryHandler : IQueryHandler<DetailsQuery, TodoDetails>
	{
		private readonly AsyncPoco.Database _database;

		private const string UserSql = @"
SELECT UserName, DisplayName, COUNT(Todo.Id) AS TodoItemCount 
		FROM [User] LEFT JOIN [Todo] ON [User].Id = AssignedToUserId
		GROUP BY UserName, DisplayName";

		public DetailsQueryHandler(Database database)
		{
			_database = database;
		}

		public async Task<TodoDetails> RequestAsync(DetailsQuery query)
		{
			var todo = await _database.SingleAsync<TodoEntity>(query.Id);
			var assignedTo = await _database.SingleAsync<UserEntity>(todo.AssignedToUserId);
			
			var details = Mapping.MapTo<TodoDetails>(todo, assignedTo);

			var users = await _database.FetchAsync<AssignedTo>(UserSql);
			details.Users = users.ToList();

			return details;
		}
	}
}