using System;

namespace Lab1_.NET.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool Important { get; set; }

        public DateTime DateTime { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
