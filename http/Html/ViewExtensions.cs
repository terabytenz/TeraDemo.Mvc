using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Terademo.Mvc.Html
{
	public static class ViewExtensions
	{
		public static ViewDataDictionary AsViewData<T>(this T model, params Expression<Func<T, object>>[] propertySelectors)
		{
			var result = new ViewDataDictionary {{"ParentModel", model}};

			foreach (var expression in propertySelectors)
			{
				var propertyExpression = (MemberExpression) expression.Body;
				var selector = expression.Compile();
				result.Add(propertyExpression.Member.Name, selector(model));
			}
			
			return result;
		}
	}
}