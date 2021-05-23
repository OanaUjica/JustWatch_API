using System;
using System.Collections.Generic;

namespace Lab1_.NET.Models
{
    public class Movie
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        //[Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        //[Required]
        //[EnumDataType(typeof(MovieGenre), ErrorMessage = "Please enter a genre.")]
        public string Genre { get; set; }

        public ushort? DurationInMinutes { get; set; }
                
        public ushort YearOfRelease { get; set; }

        public string Director { get; set; }

        public DateTime DateAdded { get; set; }

        //[Range(1, 10)]
        public float Rating { get; set; }

        public bool Watched { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Reservation> Reservations { get; set; }
    }

    public enum MovieGenre
    {
        Action,
        Comedy,
        Horror,
        Thriller
    }
}
