using System;
using System.Collections.Generic;

namespace Lab1_.NET.ViewModels.Reservations
{
    public class NewReservationRequest
    {
        public List<int> ReservedMovieIds { get; set; }

        public DateTime? ReservationDateTime { get; set; }
    }
}
