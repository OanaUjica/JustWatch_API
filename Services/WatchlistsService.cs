using AutoMapper;
using Lab1_.NET.Data;
using Lab1_.NET.ErrorHandling;
using Lab1_.NET.Models;
using Lab1_.NET.ViewModels;
using Lab1_.NET.ViewModels.Reservations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1_.NET.Services
{
    public class WatchlistsService : IWatchlistsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WatchlistsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PaginatedResultSet<WatchlistsForUserResponse>, IEnumerable<EntityError>>> GetAllWatchlists(ApplicationUser user, int? page = 1, int? perPage = 5)
        {
            var watchlistsFromDb = await _context.Watchlists
                .Where(o => o.ApplicationUser.Id == user.Id)
                .Include(o => o.Movies)
                .OrderBy(w => w.Id)
                .Skip((page.Value - 1) * perPage.Value)
                .Take(perPage.Value)
                .ToListAsync();

            if (page == null || page < 1)
            {
                page = 1;
            }
            if (perPage == null || perPage > 100)
            {
                perPage = 20;
            }

            var reservationsForUserResponse = _mapper.Map<List<Watchlist>, List<WatchlistsForUserResponse>>(watchlistsFromDb);

            int count = await _context.Movies.CountAsync();
            var resultSet = new PaginatedResultSet<WatchlistsForUserResponse>(reservationsForUserResponse, page.Value, count, perPage.Value);

            var serviceResponse = new ServiceResponse<PaginatedResultSet<WatchlistsForUserResponse>, IEnumerable<EntityError>>
            {
                ResponseOk = resultSet
            };

            return serviceResponse;
        }

        public async Task<ServiceResponse<Watchlist, IEnumerable<EntityError>>> PlaceWatchlists(NewWatchlistRequest newWatchlistRequest, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<Watchlist, IEnumerable<EntityError>>();

            var reservedMovies = new List<Movie>();
            newWatchlistRequest.WatchlistMovieIds.ForEach(rid =>
            {
                var movieWithId = _context.Movies.Find(rid);
                if (movieWithId != null)
                {
                    reservedMovies.Add(movieWithId);
                }
            });

            var watchlist = new Watchlist
            {
                ApplicationUser = user,
                WatchlistDateAdded = newWatchlistRequest.WatchlistDateAdded.GetValueOrDefault(),
                Movies = reservedMovies
            };

            _context.Watchlists.Add(watchlist);

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = watchlist;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>
                {
                    new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message }
                };
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Watchlist, IEnumerable<EntityError>>> UpdateWatchlist(int id, NewWatchlistRequest updateReservationRequest, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<Watchlist, IEnumerable<EntityError>>();

            var watchlistMovies = new List<Movie>();
            updateReservationRequest.WatchlistMovieIds.ForEach(rid =>
            {
                var movieWithId = _context.Movies.Find(rid);
                if (movieWithId != null)
                {
                    watchlistMovies.Add(movieWithId);
                }
            });

            var watchlist = new Watchlist
            {
                Id = id,
                ApplicationUser = user,
                WatchlistDateAdded = updateReservationRequest.WatchlistDateAdded.GetValueOrDefault(),
                Movies = watchlistMovies
            };

            _context.Entry(watchlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = watchlist;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>
                {
                    new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message }
                };
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteMovieFromWatchlist(int id, string userId)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<EntityError>>();

            try
            {
                var watchlists = _context.Watchlists.Where(o => o.ApplicationUser.Id == userId).Include(o => o.Movies).ToList();

                var watchlist = await _context.Watchlists.FindAsync(id);
                _context.Watchlists.Remove(watchlist);
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = true;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>
                {
                    new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message }
                };
                serviceResponse.ResponseError = errors;
            }

            return serviceResponse;
        }

        public bool WatchlistExists(int id)
        {
            return _context.Watchlists.Any(e => e.Id == id);
        }
    }
}
