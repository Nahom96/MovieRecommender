using MovieRecommender.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MovieRecommender.Services
{

	public class MovieService
	{
		private readonly string _apiKey;
		private readonly string _baseUrl;

		public MovieService(IConfiguration configuration)
		{
			_apiKey = configuration["TMDb:ApiKey"];
			_baseUrl = configuration["TMDb:BaseUrl"];
		}

		// Fetch popular movies
		public async Task<List<Movie>> GetPopularMoviesAsync()
		{
			var client = new RestClient($"{_baseUrl}/movie/popular?api_key={_apiKey}");
			var request = new RestRequest();
			var response = await client.ExecuteAsync(request);

			if (response.IsSuccessful)
			{
				var movieResult = JsonConvert.DeserializeObject<MovieResult>(response.Content);
				return movieResult?.Results ?? new List<Movie>();
			}

			return new List<Movie>();
		}

		// Fetch movie details
		public async Task<Movie> GetMovieDetailsAsync(int movieId)
		{
			var client = new RestClient($"{_baseUrl}/movie/{movieId}?api_key={_apiKey}");
			var request = new RestRequest();
			var response = await client.ExecuteAsync(request);

			if (response.IsSuccessful)
			{
				var apiResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);

				return new Movie
				{
					Id = apiResponse.id,
					Title = apiResponse.title,
					Overview = apiResponse.overview,
					PosterPath = apiResponse.poster_path,
					ReleaseDate = apiResponse.release_date,
					VoteAverage = apiResponse.vote_average,
					Genres = ((IEnumerable<dynamic>)apiResponse.genres)
						.Select(g => (string)g.name)
						.ToList()
				};
			}

			return null;
		}

		// Fetch genres
		public async Task<List<Genre>> GetGenresAsync()
		{
			var client = new RestClient($"{_baseUrl}/genre/movie/list?api_key={_apiKey}");
			var request = new RestRequest();
			var response = await client.ExecuteAsync(request);

			if (response.IsSuccessful)
			{
				var genreResult = JsonConvert.DeserializeObject<GenreResult>(response.Content);
				return genreResult?.Genres ?? new List<Genre>();
			}

			return new List<Genre>();
		}
	}
}
