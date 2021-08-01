using DummyNetflixApi.entities;
using DummyNetflixApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DummyNetflixApi.DTO
{
    public class MovieDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public int GenreId { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Movie ToMovie()
        {
            var (header, bytes) = GetImageForMoview(ImageBase64);
            var movie = new Movie()
            {
                Id = Guid.NewGuid().ToString(),
                Description = Description, 
                Title = Title, 
                ReleaseDate = ReleaseDate,
                ImageHeaderBase64 = header,
                ImageBase64 = bytes
            };
            return movie;
        }

        private (string, byte[]) GetImageForMoview(string imageBase64)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ImageBase64))
                {
                    var commaIndex = ImageBase64.IndexOf(',');
                    if (commaIndex != -1)
                    {
                        var header = imageBase64.Substring(0, commaIndex + 1);
                        var restBase64 = imageBase64.Substring(commaIndex + 1, imageBase64.Length - commaIndex -1 );
                        return (imageBase64.Substring(0, commaIndex + 1), Convert.FromBase64String(restBase64));
                    }
                }
                return (null, null);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }

        public void UpdateMovie(Movie movie)
        {
            movie.Title = Title;
            movie.Description = Description;
            movie.ImageBase64 = Convert.FromBase64String(ImageBase64);
        }

        public static MovieDTO CreateFromMovie(Movie movie)
        {
            return new MovieDTO() { 
                Id = movie.Id, 
                Title = movie.Title, 
                Description = movie.Description, 
                ImageBase64 = movie.ImageBase64 != null && movie.ImageHeaderBase64 != null ? movie.ImageHeaderBase64 + Convert.ToBase64String(movie.ImageBase64) : null,
                GenreId = movie.Genre.Id,
                ReleaseDate = movie.ReleaseDate};
        }
    }
}
