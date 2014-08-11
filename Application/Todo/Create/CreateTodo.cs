using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Application.Data;
using Application.Mediator;
using Terabyte.Mapper;

namespace Application.Todo.Create
{
	public class CreateTodo : ICommand<UnitType>
	{
		[Required]
		public string Title { get; set; }

		public string AssignedTo { get; set; }
	}

	public class CreateTodoMappingProvider : IMappingProvider
	{
		public void RegisterTypeMappings(AutoMapper.IConfiguration config)
		{
			config.CreateMap<CreateTodo, TodoEntity>();
		}
	}
}