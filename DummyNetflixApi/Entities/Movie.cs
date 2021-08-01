using DummyNetflixApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DummyNetflixApi.entities
{
    public class Movie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] ImageBase64 { get; set; }
        public string ImageHeaderBase64 { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Genre Genre { get; set; }

        public void UpdateMovie(Movie movie)
        {
            Title = movie.Title;
            Description = movie.Description;
            ImageBase64 = movie.ImageBase64;
        }

    }

    public static class MovieHelper
    {
        public static void UpdateMovie(this Movie oldMovie, Movie newMovie)
        {
            oldMovie.Title = newMovie.Title;
            oldMovie.Title = newMovie.Title;
            oldMovie.Title = newMovie.Title;
            oldMovie.Title = newMovie.Title;
        }
    }
}
