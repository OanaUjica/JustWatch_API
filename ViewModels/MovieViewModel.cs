using Lab1_.NET.Models;
using System;
using System.Collections.Generic;

namespace Lab1_.NET.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public MovieGenre Genre { get; set; }

        public ushort? DurationInMinutes { get; set; }

        public ushort YearOfRelease { get; set; }

        public string Director { get; set; }

        public DateTime DateAdded { get; set; }

        public float Rating { get; set; }

        public bool Watched { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
