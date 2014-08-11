using System;
using System.Linq;
using System.Threading.Tasks;

namespace Specifications
{
	public static class TaskSpecificationExtensions
	{
		public static T Await<T>(this Task<T> task)
		{
			return task.Result;
		}
	}

	public static class AsyncCatch
	{
		public static void Exception(Func<Task> func, out Exception exception)
		{
			try
			{
				exception = null;
				var task = func();
				task.Wait();
			}
			catch (AggregateException e)
			{
				exception = e.InnerExceptions.First();
			}
		}
	}
}