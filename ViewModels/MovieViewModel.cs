using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1_.NET.ViewModels
{
    public class MovieViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Genre Genre { get; set; }

        public string DurationInMinutes { get; set; }

        public ushort YearOfRelease { get; set; }

        public string Director { get; set; }

        public DateTime DateAdded { get; set; }

        public float? Rating { get; set; }

        public bool Watched { get; set; }
    }

    public enum Genre
    {
        Action,
        Comedy,
        Horror,
        Thriller
    }
}
