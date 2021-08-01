using DummyNetflixApi.Database;
using DummyNetflixApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace DummyNetflixApi.Controllers
{
    [Route("api/genres")]
    [Authorize]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly DummyNetflixDbContext _dummyNetflixDbContex;

        public GenresController(DummyNetflixDbContext dummyNetflixDbContex)
        {
            _dummyNetflixDbContex = dummyNetflixDbContex;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            return Ok(await _dummyNetflixDbContex.Genries.AsQueryable().ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(Genre genre)
        {
            var currentGenre = await _dummyNetflixDbContex.Genries.FindAsync(genre.Id);
            if (currentGenre == null)
                _dummyNetflixDbContex.Genries.Add(genre);
            else
                currentGenre.Name = genre.Name;

            await _dummyNetflixDbContex.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = _dummyNetflixDbContex.Genries.Attach(new Genre() { Id = id });
            _dummyNetflixDbContex.Remove(obj);
            await _dummyNetflixDbContex.SaveChangesAsync();
            return Ok();
        }
    }
}
