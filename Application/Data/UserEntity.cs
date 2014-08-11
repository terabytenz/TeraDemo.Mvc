using System;
using System.Collections.Generic;
using System.Linq;
using AsyncPoco;

namespace Application.Data
{
	[TableName("User")]
	[PrimaryKey("Id", autoIncrement = true)]
	public class UserEntity
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string DisplayName { get; set; }
	}
}