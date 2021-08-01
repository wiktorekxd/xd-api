using DummyNetflixApi.Database;
using DummyNetflixApi.DTO;
using DummyNetflixApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DummyNetflixApi.Controllers
{
    [Route("api/popular-movies")]
    [ApiController]
    public class PopularMoviesController : ControllerBase
    {
        private readonly DummyNetflixDbContext _dummyNetflixDbContex;

        public PopularMoviesController(DummyNetflixDbContext dummyNetflixDbContex)
        {
            _dummyNetflixDbContex = dummyNetflixDbContex;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var maxRandomGenresLength = 3;
            var genres = await _dummyNetflixDbContex.Genries.AsQueryable().ToListAsync();
            var randomGenres = new List<Genre>();

            Random random = new Random();
            for (var i = 0; i < maxRandomGenresLength && genres.Count > 0; i++)
            {
                var randomIndex = random.Next(0, genres.Count -1);
                var genre = genres[randomIndex];
                randomGenres.Add(genre);
                genres.Remove(genre);
            }

            var randomGenresId = randomGenres.Select(x => x.Id);

            var genreMovieList = await _dummyNetflixDbContex.Movies.Where(x => randomGenresId.Contains(x.Genre.Id) &&
                _dummyNetflixDbContex.Movies.Where(y => y.Genre.Id == x.Genre.Id && y.ReleaseDate > x.ReleaseDate).Count() < 2).ToListAsync();

            var genreMovieDTOList = genreMovieList.Select(x => MovieDTO.CreateFromMovie(x));
            return Ok(genreMovieDTOList);

        }
        [HttpGet("main")]
        public async Task<IActionResult> GetMainMovie()
        {
            var count = await _dummyNetflixDbContex.Movies.AsQueryable().CountAsync();
            Random random = new Random();
            var rnd = random.Next(0, count - 1);
            var movie = await _dummyNetflixDbContex.Movies.AsQueryable().Skip(rnd).Take(1).ToListAsync();
            return Ok(movie.First());
        }
    }
}
