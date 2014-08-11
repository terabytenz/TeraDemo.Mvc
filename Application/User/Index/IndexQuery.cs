using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.Infrastructure;
using Application.Mediator;
using AsyncPoco;

namespace Application.User.Index
{
	public class IndexQuery : IQuery<Index>
	{
	}

	public class IndexQueryHandler : IQueryHandler<IndexQuery, Index>
	{
		private readonly Database _database;

		public IndexQueryHandler(Database database)
		{
			_database = database;
		}

		public async Task<Index> RequestAsync(IndexQuery query)
		{
			var entities = await _database.FetchAsync<UserEntity>("");
			var users = entities.MapAll<User>().ToList();
			
			return new Index
			{
				Users = users.ToList()
			};
		}
	}
}