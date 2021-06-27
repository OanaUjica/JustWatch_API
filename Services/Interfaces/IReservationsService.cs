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
    public interface IReservationsService
    {
        Task<ServiceResponse<Reservation, IEnumerable<EntityError>>> PlaceReservation(NewReservationRequest newReservationRequest, ApplicationUser user);

        Task<ServiceResponse<PaginatedResultSet<ReservationsForUserResponse>, IEnumerable<EntityError>>> GetAllReservations(ApplicationUser user, int? page = 1, int? perPage = 5);

        Task<ServiceResponse<Reservation, IEnumerable<EntityError>>> UpdateReservation(int id, NewReservationRequest updateReservationRequest, ApplicationUser user);

        Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteReservation(int id);

        bool ReservationExists(int id);
    }
}
