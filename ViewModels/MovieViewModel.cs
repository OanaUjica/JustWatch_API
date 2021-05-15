using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Lab1_.NET.ViewModels
{
    public class MovieViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }


        [EnumDataType(typeof(MovieGenre), ErrorMessage = "Please enter a genre.")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MovieGenre Genre { get; set; }

        public ushort? DurationInMinutes { get; set; }

        public ushort YearOfRelease { get; set; }

        public string Director { get; set; }

        public DateTime DateAdded { get; set; }

        [Range(1, 10)]
        public float Rating { get; set; }

        public bool Watched { get; set; }
    }

    public enum MovieGenre
    {
        [EnumMember(Value = "Action")]
        Action,
        [EnumMember(Value = "Comedy")]
        Comedy,
        [EnumMember(Value = "Horror")]
        Horror,
        [EnumMember(Value = "Thriller")]
        Thriller
    }
}
