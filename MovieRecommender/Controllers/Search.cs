using Microsoft.AspNetCore.Mvc;

namespace MovieRecommender.Controllers
{
	public class Search : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
