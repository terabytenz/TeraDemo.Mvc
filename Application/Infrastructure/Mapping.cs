using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Application.Infrastructure
{
	public static class Mapping
	{
		private static IMappingEngine _mappingEngine;

		public static void SetMappingEngine(IMappingEngine mappingEngine)
		{
			_mappingEngine = mappingEngine;
		}

		public static IEnumerable<T> MapAll<T>(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return Enumerable.Empty<T>();
			}

			return Map<List<T>>(enumerable);
		}

		public static T Map<T>(this object value)
		{
			var mapper = GetMapper();
			return mapper.Map<T>(value);
		}

		public static T MapTo<T>(params object[] mapFrom)
		{
			var mapper = GetMapper();
			return (T) mapFrom.Aggregate((object) null, (current, x) => mapper.Map(x, current, x.GetType(), typeof (T)));
		}

		private static IMappingEngine GetMapper()
		{
			if (_mappingEngine == null)
			{
				throw new InvalidOperationException("No mapper registered");
			}

			return _mappingEngine;
		}
	}
}