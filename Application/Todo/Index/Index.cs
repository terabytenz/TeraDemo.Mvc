using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Todo.Index
{
	public class Index
	{
		public IEnumerable<TodoItem> Items { get; set; }
		public IEnumerable<AssignedTo> Users { get; set; }
	}
}