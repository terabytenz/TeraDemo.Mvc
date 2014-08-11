using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Terademo.Mvc.Html
{
	public static class SelectItemHelper
	{
		public static IEnumerable<SelectListItem> From<T>(IEnumerable<T> items, Func<T, string> selectValue, Func<T, string> selectText = null)
		{
			selectText = selectText ?? selectValue;

			return
				from item in items
				select new SelectListItem { Value = selectValue(item), Text = selectText(item) };
		}

		public static IEnumerable<SelectListItem> Selected(this IEnumerable<SelectListItem> items, string value)
		{
			return 
				from item in items
				select new SelectListItem { Value = item.Value, Text = item.Text, Selected = item.Value == value };
		}
	}
}