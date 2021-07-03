using System;
using System.Collections.Generic;

namespace Lab1_.NET.ViewModels.Reservations
{
    public class WatchlistsForUserResponse
    {
        public int Id { get; set; }
        public ApplicationUserViewModel ApplicationUser { get; set; }

        public List<MovieViewModel> Movies { get; set; }

        public DateTime WatchlistDateAdded { get; set; }
    }
}
