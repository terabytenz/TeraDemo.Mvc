using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;

namespace Application.Mediator
{
	public class Mediator : IMediator
	{
		private readonly IComponentContext _componentContext;

		public Mediator(IComponentContext componentContext)
		{
			_componentContext = componentContext;
		}

		public async Task<TResponse> RequestAsync<TResponse>(IQuery<TResponse> query)
		{
			var plan = new MediatorPlan<TResponse>(typeof (IQueryHandler<,>), "RequestAsync", query.GetType(), _componentContext);
			return await plan.InvokeAsync(query);
		}

		public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> query)
		{
			var plan = new MediatorPlan<TResponse>(typeof(ICommandHandler<,>), "SendAsync", query.GetType(), _componentContext);
			return await plan.InvokeAsync(query);
		}

		private class MediatorPlan<TResponse>
        {
            private readonly MethodInfo _handleMethod;
            private readonly object _handlerInstance;

            public MediatorPlan(Type handlerTypeTemplate, string handlerMethodName, Type messageType, IComponentContext componentContext)
            {
                var handlerType = handlerTypeTemplate.MakeGenericType(messageType, typeof (TResponse));
                _handleMethod = GetHandlerMethod(handlerType, handlerMethodName, messageType);

	            if (!componentContext.TryResolve(handlerType, out _handlerInstance))
	            {
					throw new InvalidOperationException(string.Format("Failed to find a handler for {0} => {1}", messageType.Name, typeof(TResponse).Name));
	            }
            }

            MethodInfo GetHandlerMethod(Type handlerType, string handlerMethodName, Type messageType)
            {
                return handlerType
                    .GetMethod(handlerMethodName,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                        null, CallingConventions.HasThis,
                        new[] { messageType },
                        null);
            }

            public async Task<TResponse> InvokeAsync(object message)
            {
                return await (Task<TResponse>) _handleMethod.Invoke(_handlerInstance, new[] { message });
            }
        }
	}
}