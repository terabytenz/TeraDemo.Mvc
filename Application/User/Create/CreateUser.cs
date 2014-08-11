using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Application.Data;
using Application.Mediator;
using Terabyte.Mapper;

namespace Application.User.Create
{
	public class CreateUser : ICommand<UnitType>
	{
		[Required]
		[DisplayName("User name")]
		public string UserName { get; set; }

		[Required]
		[DisplayName("Name")]
		public string DisplayName { get; set; }
	}

	public class CreateUserMappingProvider : IMappingProvider
	{
		public void RegisterTypeMappings(AutoMapper.IConfiguration config)
		{
			config.CreateMap<CreateUser, UserEntity>();
		}
	}
}