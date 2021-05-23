using System;
using System.Collections.Generic;

namespace Lab1_.NET.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public DateTime ReservationDateTime { get; set; }
    }
}
