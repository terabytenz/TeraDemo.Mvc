using System;
using System.Collections.Generic;
using System.Linq;
using Application.Mediator;

namespace Application.Todo.Assign
{
	public class AssignTodo : ICommand<UnitType>
	{
		public int Id { get; set; }
		public string AssignTo { get; set; }
	}
}