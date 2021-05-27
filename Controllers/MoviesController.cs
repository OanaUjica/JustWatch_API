using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab1_.NET.Data;
using Lab1_.NET.Models;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Lab1_.NET.ViewModels;

namespace Lab1_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MoviesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            if (!fromDate.HasValue || !toDate.HasValue)
            {
                return BadRequest("Both dates are required");
            }

            if (fromDate >= toDate)
            {
                return BadRequest("fromDate is not before toDate");
            }

            var filteredMovies = await _context.Movies
                .Where(m => m.DateAdded >= fromDate && m.DateAdded <= toDate)
                .OrderByDescending(m => m.YearOfRelease)
                .Include(m => m.Comments)
                .Select(m => _mapper.Map<MovieViewModel>(m))
                .ToListAsync();

            return Ok(filteredMovies);
        }

        /// <summary>
        /// Get movie with comments
        /// </summary>
        /// <response code="200">Get movie with comments</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}/Comments")]
        public async Task<ActionResult<IEnumerable<MovieWithCommentsViewModel>>> GetCommentsForMovie(int id)
        {
            var moviesWithComments = await _context.Movies
                .Where(m => m.Id == id)
                .Include(m => m.Comments)
                .Select(m => _mapper.Map<MovieWithCommentsViewModel>(m))
                .ToListAsync();

            return Ok(moviesWithComments);
        }

        /// <summary>
        /// Add a new comment to movie
        /// </summary>
        /// <response code="201">Add a new comment to movie</response>
        /// <response code="404">Movie not found</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{id}/Comments")]
        public async Task<ActionResult> PostCommentForMovie(int id, CommentViewModel commentVM)
        {
            var commentDB = _mapper.Map<Comment>(commentVM);

            var movie = await _context.Movies
                .Where(m => m.Id == id)
                .Include(m => m.Comments)
                .FirstOrDefaultAsync();
            if (movie == null)
            {
                return NotFound();
            }

            movie.Comments.Add(commentDB);
            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovieWithComments", new { id = commentDB.Id }, "New comment successfully added");
        }

        /// <summary>
        /// Get a list of movies
        /// </summary>
        /// <response code="200">Get a list of movies</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> Getmovies()
        {
            var movies =  await _context.Movies
                .Include(m => m.Comments)
                .Select(m => _mapper.Map<MovieViewModel>(m))
                .ToListAsync();

            return Ok(movies);
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
            var movie = await _context.Movies
                .Where(m => m.Id == id)
                .Include(m => m.Comments)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                return NotFound();
            }

            var movieVM = _mapper.Map<MovieViewModel>(movie);

            return Ok(movieVM);
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
        public async Task<IActionResult> PutMovie(int id, MovieViewModel movieVM)
        {
            var movie = _mapper.Map<Movie>(movieVM);

            if (movie == null)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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
        /// Creates new movie
        /// </summary>
        /// <response code="201">Creates new movie</response>
        /// <response code="400">Unable to create the movie due to validation error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieViewModel>> PostMovie([FromBody] MovieViewModel movieRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = _mapper.Map<Movie>(movieRequest);
            _context.Movies.Add(movie);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, "New movie successfully created");
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
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
