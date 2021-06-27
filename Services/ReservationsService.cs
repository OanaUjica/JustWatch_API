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
    public class ReservationsService : IReservationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReservationsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PaginatedResultSet<ReservationsForUserResponse>, IEnumerable<EntityError>>> GetAllReservations(ApplicationUser user, int? page = 1, int? perPage = 5)
        {
            var reservationsFromDb = await _context.Reservations
                .Where(o => o.ApplicationUser.Id == user.Id)
                .Include(o => o.Movies)
                .OrderBy(m => m.Id)
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

            var reservationsForUserResponse = _mapper.Map<List<Reservation>, List<ReservationsForUserResponse>>(reservationsFromDb);

            int count = await _context.Movies.CountAsync();
            var resultSet = new PaginatedResultSet<ReservationsForUserResponse>(reservationsForUserResponse, page.Value, count, perPage.Value);
                        
            var serviceResponse = new ServiceResponse<PaginatedResultSet<ReservationsForUserResponse>, IEnumerable<EntityError>>();
            serviceResponse.ResponseOk = resultSet;

            return serviceResponse;
        }

        public async Task<ServiceResponse<Reservation, IEnumerable<EntityError>>> PlaceReservation(NewReservationRequest newReservationRequest, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<Reservation, IEnumerable<EntityError>>();

            var reservedMovies = new List<Movie>();
            newReservationRequest.ReservedMovieIds.ForEach(rid =>
            {
                var movieWithId = _context.Movies.Find(rid);
                if (movieWithId != null)
                {
                    reservedMovies.Add(movieWithId);
                }
            });

            var reservation = new Reservation
            {
                ApplicationUser = user,
                ReservationDateTime = newReservationRequest.ReservationDateTime.GetValueOrDefault(),
                Movies = reservedMovies
            };

            _context.Reservations.Add(reservation);

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = reservation;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Reservation, IEnumerable<EntityError>>> UpdateReservation(int id, NewReservationRequest updateReservationRequest, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<Reservation, IEnumerable<EntityError>>();

            var reservedMovies = new List<Movie>();
            updateReservationRequest.ReservedMovieIds.ForEach(rid =>
            {
                var movieWithId = _context.Movies.Find(rid);
                if (movieWithId != null)
                {
                    reservedMovies.Add(movieWithId);
                }
            });

            var reservation = new Reservation
            {
                Id = id,
                ApplicationUser = user,
                ReservationDateTime = updateReservationRequest.ReservationDateTime.GetValueOrDefault(),
                Movies = reservedMovies
            };

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = reservation;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteReservation(int id)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<EntityError>>();

            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = true;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
                serviceResponse.ResponseError = errors;
            }

            return serviceResponse;
        }

        public bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
