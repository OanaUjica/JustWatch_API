using Lab1_.NET.ErrorHandling;
using Lab1_.NET.Models;
using Lab1_.NET.ViewModels;
using Lab1_.NET.ViewModels.Reservations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1_.NET.Services
{
    public interface IWatchlistsService
    {
        Task<ServiceResponse<PaginatedResultSet<WatchlistsForUserResponse>, IEnumerable<EntityError>>> GetAllWatchlists(ApplicationUser user, int? page = 1, int? perPage = 5);

        Task<ServiceResponse<Watchlist, IEnumerable<EntityError>>> PlaceWatchlists(NewWatchlistRequest newWatchlistRequest, ApplicationUser user);

        Task<ServiceResponse<Watchlist, IEnumerable<EntityError>>> UpdateWatchlist(int id, NewWatchlistRequest updateWatchlistRequest, ApplicationUser user);

        Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteWatchlist(int id);

        bool WatchlistExists(int id);
    }
}
