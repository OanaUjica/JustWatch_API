using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Lab1_.NET.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Genre { get; set; }

        public ushort? DurationInMinutes { get; set; }
                
        public ushort YearOfRelease { get; set; }

        public string Director { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }

        public float Rating { get; set; }

        public bool Watched { get; set; }
    }
}
