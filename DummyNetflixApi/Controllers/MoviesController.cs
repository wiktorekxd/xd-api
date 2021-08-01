using DummyNetflixApi.Database;
using DummyNetflixApi.DTO;
using DummyNetflixApi.entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DummyNetflixApi.controllers
{
    [Route("api/movies")]
    [Authorize]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly DummyNetflixDbContext _dummyNetflixDbContext;

        public MoviesController(DummyNetflixDbContext dummyNetflixDbContext)
        {
            _dummyNetflixDbContext = dummyNetflixDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var moviesDTO = (await _dummyNetflixDbContext.Movies.AsQueryable().ToListAsync()).Select(m=> MovieDTO.CreateFromMovie(m));
            return Ok(moviesDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(string id)
        {
            var movieEntity = await _dummyNetflixDbContext.Movies.FindAsync(id);

            if (movieEntity == null)
                return NotFound();

            var movieDTO = MovieDTO.CreateFromMovie(movieEntity);
            return Ok(movieDTO);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateMovie(MovieDTO movieDTO)
        {
            var movie = movieDTO.ToMovie();
            var genre = await _dummyNetflixDbContext.Genries.FindAsync(movieDTO.GenreId);
            movie.Genre = genre;
            await _dummyNetflixDbContext.Movies.AddAsync(movie);
            await _dummyNetflixDbContext.SaveChangesAsync();
            return Ok(movieDTO);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovie(Movie newMovie)
        {
            var movie = await _dummyNetflixDbContext.Movies.FindAsync(newMovie.Id);
            if (movie == null)
                return NotFound();

            newMovie.UpdateMovie(movie);
            await _dummyNetflixDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            var movie = await _dummyNetflixDbContext.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            _dummyNetflixDbContext.Movies.Remove(movie);
            await _dummyNetflixDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
