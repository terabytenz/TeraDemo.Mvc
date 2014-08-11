using System;
using System.Collections.Generic;
using System.Linq;
using Application.Mediator;

namespace Application.Todo.Complete
{
	public class CompleteTodo : ICommand<UnitType>
	{
		public int Id { get; set; }
	}
}