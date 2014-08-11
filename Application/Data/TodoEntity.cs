using System;
using System.Collections.Generic;
using System.Linq;
using AsyncPoco;

namespace Application.Data
{
	[TableName("Todo")]
	[PrimaryKey("Id", autoIncrement = true)]
	public class TodoEntity
	{
		public TodoEntity()
		{
			Description = string.Empty;
			CreatedOn = DateTime.Now;
			UpdatedAt = DateTime.Now;
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsComplete { get; set; }

		public int AssignedToUserId { get; set; }

		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}