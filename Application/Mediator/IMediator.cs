using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Mediator
{
	public interface IMediator
	{
		Task<TResponse> RequestAsync<TResponse>(IQuery<TResponse> query);
		Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> query);
	}

	public interface IQueryHandler<in TQuery, TResponse>
		where TQuery : IQuery<TResponse>
	{
		Task<TResponse> RequestAsync(TQuery query);
	}

	public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, UnitType>
		where TCommand : ICommand<UnitType>
	{ }

	public interface ICommandHandler<in TCommand, TResponse>
		where TCommand : ICommand<TResponse>
	{
		Task<TResponse> SendAsync(TCommand command);
	}

	public interface IQuery<out TResponse>
	{
	}

	public interface ICommand<out TResponse>
	{
	}

	public class UnitType
	{
		public static readonly UnitType Default = new UnitType();
		private UnitType()
		{
		}
	}
}