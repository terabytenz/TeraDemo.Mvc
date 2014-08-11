using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Todo
{
	public class AssignedTo
	{
		public string UserName { get; set; }
		public string DisplayName { get; set; }

		public int TodoItemCount { get; set; }

		public bool IsUnassignedUser { get { return string.IsNullOrEmpty(UserName); }}
	}
}