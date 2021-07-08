using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lab1_.NET.Models;
using Microsoft.AspNetCore.Http;
using Lab1_.NET.ViewModels;
using Lab1_.NET.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Lab1_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(IMoviesService moviesService, UserManager<ApplicationUser> userManager)
        {
            _moviesService = moviesService;
            _userManager = userManager;
        }

        /// <summary>
        /// Get a list of movies
        /// </summary>
        /// <response code="200">Get a list of movies</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> GetMovies(int? page = 1, int? perPage = 5)
        {
            var moviesServiceResult = await _moviesService.GetMovies(page, perPage);

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Get a movie by id
        /// </summary>
        /// <response code="200">Get a movie by id</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieViewModel>> GetMovie(int id)
        {
            var moviesServiceResult = await _moviesService.GetMovie(id);

            if (moviesServiceResult.ResponseOk == null)
            {
                return NotFound();
            }

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Get movie with comments
        /// </summary>
        /// <response code="200">Get movie with comments</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}/Reviews")]
        public async Task<ActionResult<IEnumerable<MovieWithReviewsViewModel>>> GetReviewsForMovie(int id, int? page = 1, int? perPage = 5)
        {
            if (!_moviesService.MovieExists(id))
            {
                return NotFound();
            }

            var movieResponse = await _moviesService.GetMovie(id);
            if (movieResponse.ResponseOk == null)
            {
                return NotFound();
            }

            var moviesServiceResult = await _moviesService.GetReviewsForMovie(id, page, perPage);

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Filter movies by date added
        /// </summary>
        /// <response code="200">Filter movies by date added</response>
        /// <response code="400">Unable to get the movie due to validation error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Route("filter")]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> FilterMoviesByDateAdded(DateTime? fromDate, DateTime? toDate)
        {
            var moviesServiceResult = await _moviesService.FilterMoviesByDateAdded(fromDate, toDate);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return Ok(moviesServiceResult.ResponseOk);
        }

        /// <summary>
        /// Add a new movie
        /// </summary>
        /// <response code="201">Add a new movie</response>
        /// <response code="400">Unable to create the movie due to validation error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult<MovieViewModel>> PostMovie([FromBody] MovieViewModel movieRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var moviesServiceResult = await _moviesService.PostMovie(movieRequest);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            var movie = moviesServiceResult.ResponseOk;

            return CreatedAtAction("GetMovie", new { id = movie.Id }, "New movie successfully created");
        }

        /// <summary>
        /// Add a new comment to movie
        /// </summary>
        /// <response code="201">Add a new comment to movie</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{movieId}/Reviews")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult> PostReviewForMovie(int movieId, ReviewViewModel reviewRequest)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var moviesServiceResult = await _moviesService.PostReviewForMovie(movieId, reviewRequest);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            var movie = moviesServiceResult.ResponseOk;

            return CreatedAtAction("GetMovieWithComments", new { id = movie.Id }, "New review successfully added");
        }

        /// <summary>
        /// Amend a movie
        /// </summary>
        /// <response code="204">Amend a movie</response>
        /// <response code="400">Unable to amend the movie due to validation error</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> PutMovie(int id, MovieViewModel movieRequest)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            if (!_moviesService.MovieExists(id))
            {
                return NotFound();
            }

            var moviesServiceResult = await _moviesService.PutMovie(id, movieRequest);
            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return NoContent();
        }

        /// <summary>
        /// Updates a review.
        /// </summary>
        /// <response code="204">If the review was successfully updated.</response>
        /// <response code="400">If the ID in the URL doesn't match the one in the body.</response>
        /// <response code="404">If the review is not found.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        [HttpPut("{movieId}/Reviews/{reviewId}")]
        public async Task<IActionResult> PutReviewForMovie(int movieId, int reviewId, ReviewViewModel reviewRequest)
        {
            if (!_moviesService.MovieExists(movieId))
            {
                return NotFound();
            }

            if (!_moviesService.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewResponse = await _moviesService.PutReviewForMovie(movieId, reviewId, reviewRequest);

            if (reviewResponse.ResponseError != null)
            {
                return BadRequest(reviewResponse.ResponseError);
            }

            return NoContent();
        }
        /// <summary>
        /// Delete a movie by id
        /// </summary>
        /// <response code="204">Delete a movie</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            if (!_moviesService.MovieExists(id))
            {
                return NotFound();
            }

            var moviesServiceResult = await _moviesService.DeleteMovie(id);

            if (moviesServiceResult.ResponseError != null)
            {
                return BadRequest(moviesServiceResult.ResponseError);
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a review from a movie.
        /// </summary>
        /// <response code="204">No content if successful.</response>
        /// <response code="404">If the review doesn't exist.</response>  
        /// <response code="400">If something goes wrong.</response>  
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("Reviews/{reviewId}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> DeleteReviewFromMovie(int reviewId)
        {
            if (!_moviesService.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var result = await _moviesService.DeleteReviewFromMovie(reviewId);

            if (result.ResponseError == null)
            {
                return NoContent();
            }

            return StatusCode(500);
        }
    }
}
