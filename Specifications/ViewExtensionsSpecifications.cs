using System.Web.Mvc;
using Application.Todo;
using Application.Todo.Index;
using FluentAssertions;
using Machine.Specifications;
using Terademo.Mvc.Html;

namespace Specifications
{
	public static class ViewExtensionsSpecifications
	{
		[Subject(typeof(ViewExtensions))]
		public class When_convert_model_to_view_data_dictionary
		{
			private static TodoItem model = new TodoItem { Title = "My Todo", AssignedTo = new AssignedTo() };
			private static ViewDataDictionary viewData;

			private Because of = () => viewData = model.AsViewData(x => x.AssignedTo);

			private It should_have_key = () => viewData.Should().ContainKey("AssignedTo");
		}
	}
}