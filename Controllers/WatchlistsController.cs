using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Lab1_.NET.Models;
using Lab1_.NET.ViewModels.Reservations;
using Microsoft.AspNetCore.Http;
using System;
using Lab1_.NET.Services;
using System.Collections.Generic;

namespace Lab1_.NET.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistsController : ControllerBase
    {
        private readonly IWatchlistsService _watchlistsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WatchlistsController(IWatchlistsService watchlistsService, UserManager<ApplicationUser> userManager)
        {
            _watchlistsService = watchlistsService;
            _userManager = userManager;
        }

        /// <summary>
        /// Add a new reservation
        /// </summary>
        /// <response code="201">Add a new reservation</response>
        /// <response code="400">Unable to add the reservation due to validation error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> PlaceWatchlist(NewWatchlistRequest newWatchlistRequest)
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var reservationServiceResult = await _watchlistsService.PlaceWatchlists(newWatchlistRequest, user);
            if (reservationServiceResult.ResponseError != null)
            {
                return BadRequest(reservationServiceResult.ResponseError);
            }

            var reservation = reservationServiceResult.ResponseOk;

            return CreatedAtAction("GetReservations", new { id = reservation.Id }, "New reservation successfully added");
        }


        /// <summary>
        /// Get reservations
        /// </summary>
        /// <response code="200">Get reservations</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchlistsForUserResponse>>> GetAllWatchlists(int? page = 1, int? perPage = 5)
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var reservationServiceResult = await _watchlistsService.GetAllWatchlists(user, page, perPage);

            return Ok(reservationServiceResult.ResponseOk);
        }

        /// <summary>
        /// Amend a reservation
        /// </summary>
        /// <response code="204">Amend a reservation</response>
        /// <response code="400">Unable to amend the reservation due to validation error</response>
        /// <response code="404">Reservation not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWatchlist(int id, NewWatchlistRequest updateReservationRequest)
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            if (!_watchlistsService.WatchlistExists(id))
            {
                return NotFound();
            }

            var reservationServiceResult = await _watchlistsService.UpdateWatchlist(id, updateReservationRequest, user);
            if (reservationServiceResult.ResponseError != null)
            {
                return BadRequest(reservationServiceResult.ResponseError);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a reservation by id
        /// </summary>
        /// <response code="204">Delete a reservation</response>
        /// <response code="404">Reservation not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWatchlist(int id)
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");                
            }

            if (!_watchlistsService.WatchlistExists(id))
            {
                return NotFound();
            }

            var reservationServiceResult = await _watchlistsService.DeleteWatchlist(id);
            if (reservationServiceResult.ResponseError != null)
            {
                return BadRequest(reservationServiceResult.ResponseError);
            }

            return NoContent();
        }


        /// <summary>
        /// Delete a reservation by id
        /// </summary>
        /// <response code="204">Delete a reservation</response>
        /// <response code="404">Reservation not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}/Movie")]
        public async Task<IActionResult> DeleteMovieFromWatchlist(int id)
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            //if (!_watchlistsService.WatchlistExists(id))
            //{
            //    return NotFound();
            //}

            var reservationServiceResult = await _watchlistsService.DeleteMovieFromWatchlist(id, user.Id);
            if (reservationServiceResult.ResponseError != null)
            {
                return BadRequest(reservationServiceResult.ResponseError);
            }

            return NoContent();
        }
    }
}
