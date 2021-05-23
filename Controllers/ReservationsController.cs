using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab1_.NET.Data;
using Lab1_.NET.Models;
using Lab1_.NET.ViewModels.Reservations;
using Microsoft.AspNetCore.Http;

namespace Lab1_.NET.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
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
        public async Task<ActionResult> PlaceReservation(NewReservationRequest newReservationRequest)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var reservedMovies = new List<Movie>();
            newReservationRequest.ReservedMovieIds.ForEach(rid =>
            {
                var movieWithId = _context.Movies.Find(rid);
                if (movieWithId != null)
                {
                    reservedMovies.Add(movieWithId);
                }
            });

            if (reservedMovies.Count == 0)
            {
                return BadRequest();
            }

            var reservation = new Reservation
            {
                ApplicationUser = user,
                ReservationDateTime = newReservationRequest.ReservationDateTime.GetValueOrDefault(),
                Movies = reservedMovies
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetMovieWithComments", new { id = reservation.Id }, "New reservation successfully added");
        }


        /// <summary>
        /// Get reservations
        /// </summary>
        /// <response code="200">Get reservations</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> GetAllReservations()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _context.Reservations
                .Where(o => o.ApplicationUser.Id == user.Id)
                .Include(o => o.Movies)
                .FirstOrDefaultAsync();

            var resultViewModel = _mapper.Map<ReservationsForUserResponse>(result);

            return Ok(resultViewModel);
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
        public async Task<IActionResult> UpdateReservation(int id, NewReservationRequest updateReservationRequest)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var reservedMovies = new List<Movie>();
            updateReservationRequest.ReservedMovieIds.ForEach(rid =>
            {
                var movieWithId = _context.Movies.Find(rid);
                if (movieWithId != null)
                {
                    reservedMovies.Add(movieWithId);
                }
            });

            if (reservedMovies.Count == 0)
            {
                return BadRequest();
            }

            //var reservation = _mapper.Map<Reservation>(reservedMovies);
            var reservation = new Reservation
            {
                Id = id,
                ApplicationUser = user,
                ReservationDateTime = updateReservationRequest.ReservationDateTime.GetValueOrDefault(),
                Movies = reservedMovies
            };

            if (reservation == null)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
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
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
