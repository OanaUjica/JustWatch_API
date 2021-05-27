using System;
using System.Collections.Generic;

namespace Lab1_.NET.ViewModels.Reservations
{
    public class ReservationsForUserResponse
    {
        public ApplicationUserViewModel ApplicationUser { get; set; }

        public List<MovieViewModel> Movies { get; set; }

        public DateTime ReservationDateTime { get; set; }
    }
}
