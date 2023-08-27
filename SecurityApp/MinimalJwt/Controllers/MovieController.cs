using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalJwt.Models;
using MinimalJwt.Services;

namespace MinimalJwt.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("Get")]
        public IResult Get(int id)
        {
            var movie = _movieService.Get(id);

            if (movie is null) return Results.NotFound("Movie not found");

            return Results.Ok(movie);
        }

        [HttpGet("List")]
        public IResult List()
        {
            var movies = _movieService.List();

            return Results.Ok(movies);
        }

        [HttpPost]
        public IResult Create(Movie movie)
        {
            var result = _movieService.Create(movie);
            return Results.Ok(result);
        }

        [HttpPost("Update")]
        public IResult Update(Movie newMovie)
        {
            var updatedMovie = _movieService.Update(newMovie);

            if (updatedMovie is null) Results.NotFound("Movie not found");

            return Results.Ok(updatedMovie);
        }

        [HttpGet("Delete")]
        public IResult Delete(int id)
        {
            var result = _movieService.Delete(id);

            if (!result) Results.BadRequest("Something went wrong");

            return Results.Ok(result);
        }
    }
}
