using System;
using System.Collections.Generic;
using System.Linq;
using Application.Data;
using AutoMapper;
using Terabyte.Mapper;

namespace Application.User.Index
{
	public class Index
	{
		public IEnumerable<User> Users { get; set; }
	}

	public class IndexMappingProvider : IMappingProvider
	{
		public void RegisterTypeMappings(IConfiguration config)
		{
			config.CreateMap<UserEntity, User>();
		}
	}
}
