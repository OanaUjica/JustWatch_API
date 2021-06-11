using Lab1_.NET.Data;
using Lab1_.NET.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lab1_.NET.Services
{
    public class MoviesService
    {
        public ApplicationDbContext _context;
        public MoviesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetAllAbovePrice()
        {
            return _context.Movies.Where(p => p.Rating >= 1).ToList();
        }
    }
}
