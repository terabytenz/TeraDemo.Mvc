using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Application.Mediator;
using Application.Todo.Assign;
using Application.Todo.Complete;
using Application.Todo.Create;
using Application.Todo.Details;
using Application.Todo.Index;

namespace Terademo.Mvc.Controllers
{
    public class TodoController : Controller
    {
		private readonly IMediator _mediator;

	    public TodoController(IMediator mediator)
	    {
		    _mediator = mediator;
	    }

	    public async Task<ActionResult> Index(IndexQuery query)
        {
			var model = await _mediator.RequestAsync(query);
			return View(model);
        }

	    public async Task<ActionResult> Create(CreateTodo item)
	    {
		    await _mediator.SendAsync(item);
		    return RedirectToAction("Index");
	    }

	    public async Task<ActionResult> Complete(CompleteTodo item)
	    {
			await _mediator.SendAsync(item);
			return RedirectToAction("Index");
		}

	    public async Task<ActionResult> Details(DetailsQuery details)
	    {
			var model = await _mediator.RequestAsync(details);
			return View(model);
	    }

		public async Task<ActionResult> Assign(AssignTodo item)
	    {
			await _mediator.SendAsync(item);
			return RedirectToAction("Details", new { id = item.Id });
		}
    }
}
