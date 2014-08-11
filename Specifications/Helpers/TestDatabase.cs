using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using AsyncPoco;
using Terademo.Mvc.Data;
using Terademo.Mvc.Data.Migrations;

namespace Specifications
{
	class TestDatabase : IDisposable
	{
		private string _dbFileName;
		private string _connectionString;
		private Database _database;

		public TestDatabase()
		{
			CreateDatabase();
		}

		public TestDatabase ApplyMigrations()
		{
			MigrationManager.CreateDatabase(_connectionString, typeof (MigrationManager).Assembly, showSql: true);
			return this;
		}

		private void CreateDatabase()
		{
			_dbFileName = Path.Combine(Environment.CurrentDirectory, string.Format("ApplicationData_{0:n}.mdf", Guid.NewGuid()));
			_connectionString = string.Format(IntegrationSettings.ConnectionString, _dbFileName);

			if (File.Exists(_dbFileName))
			{
				File.Delete(_dbFileName);
			}

			Console.WriteLine("Attaching {0}", _dbFileName);
			File.Copy(IntegrationSettings.DatabaseLocation, _dbFileName);
		}

		public string ConnectionString { get { return _connectionString; } }

		public Database Database
		{
			get
			{
				return _database ?? (_database = new AsyncPoco.Database(ConnectionString, "System.Data.SqlClient"));
			}
		}

		public async Task<bool> HasTableAsync(string tableName)
		{
			return await Database.ExecuteScalarAsync<bool>("SELECT CASE WHEN EXISTS (SELECT * FROM sys.tables WHERE name=@0) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END", tableName);
		}

		public async Task<int> CountTodoAsync(string title)
		{
			return await Database.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [Todo] WHERE Title=@0", title);
		}

		public async Task<bool> HaveTodoAsync(string title)
		{
			return await CountTodoAsync(title) > 0;
		}

		public async Task<int> CountUserAsync(string user)
		{
			return await Database.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [User] WHERE UserName=@0", user);
		}

		public async Task<bool> HaveUserAsync(string user)
		{
			return await CountUserAsync(user) > 0;
		}

		public async Task<bool> IsTodoCompleteAsync(int id)
		{
			return await Database.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [Todo] WHERE Id=@0 AND IsComplete=1", id) == 1;
		}

		public async Task<UserEntity> GetTodoAssigneeAsync(int id)
		{
			return await Database.SingleOrDefaultAsync<UserEntity>("WHERE Id=(SELECT AssignedToUserId FROM Todo WHERE Id=@0)", id);
		}

		public void Dispose()
		{
			if (File.Exists(_dbFileName))
			{
				File.Delete(_dbFileName);
			}
		}
	}
}