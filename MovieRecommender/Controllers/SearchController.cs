using Microsoft.AspNetCore.Mvc;
using MovieRecommender.Services;

namespace MovieRecommender.Controllers
{
	public class SearchController : Controller
	{
		private readonly MovieService _movieService;
		public SearchController(MovieService movieService)
		{
			_movieService = movieService;
		}

		[HttpGet("Search/Browse")]
		public async Task<IActionResult> Browse(string year = null, int? genre = null, double? rating = null, int page = 1)
		{
			var movies = await _movieService.GetFilteredMoviesAsync(year, genre, rating, page);

			// Fetch genres for the dropdown
			var genres = await _movieService.GetGenresAsync();

			// Set ViewBag properties for filters
			ViewBag.Year = year;
			ViewBag.Genre = genre;
			ViewBag.Rating = rating;
			ViewBag.Genres = genres;
			ViewBag.CurrentPage = page;

			return View(movies);
		}

	}
}
