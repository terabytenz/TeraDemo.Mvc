using System.Threading.Tasks;
using System.Web.Mvc;
using Application.Mediator;
using Application.User.Index;
using Application.User.Create;

namespace Terademo.Mvc.Controllers
{
	public class UserController : Controller
	{
		private readonly IMediator _mediator;

		public UserController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<ActionResult> Index(IndexQuery query)
		{
			var model = await _mediator.RequestAsync(query);
			return View(model);
		}

		public async Task<ActionResult> CreateUser(CreateUser model)
		{
			await _mediator.SendAsync(model);
			return RedirectToAction("Index", "Todo");
		}
	}
}