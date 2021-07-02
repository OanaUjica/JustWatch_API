using System;
using System.Collections.Generic;

namespace Lab1_.NET.ViewModels.Reservations
{
    public class NewWatchlistRequest
    {
        public List<int> WatchlistMovieIds { get; set; }

        public DateTime? WatchlistDateAdded { get; set; }
    }
}
