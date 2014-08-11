using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Terabyte.Mapper;

namespace Application.Todo.Index
{
	public class TodoItem
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public AssignedTo AssignedTo { get; set; }
	}

	public class TodoItemMappingProvider : IMappingProvider
	{
		public void RegisterTypeMappings(IConfiguration config)
		{
			config.CreateMap<IndexQueryResult, TodoItem>()
				.ForMember(x => x.AssignedTo, m => m.ResolveUsing(x => x));

			config.CreateMap<IndexQueryResult, AssignedTo>()
				.ForMember(x => x.UserName, m => m.MapFrom(x => x.AssignedToUserName))
				.ForMember(x => x.DisplayName, m => m.MapFrom(x => x.AssignedToDisplayName))
				;
		}
	}
}