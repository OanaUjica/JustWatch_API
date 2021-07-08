using Lab1_.NET.ErrorHandling;
using Lab1_.NET.Models;
using Lab1_.NET.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1_.NET.Services
{
    public interface IMoviesService
    {
        Task<ServiceResponse<PaginatedResultSet<MovieViewModel>, IEnumerable<EntityError>>> GetMovies(int? page = 1, int? perPage = 5);

        Task<ServiceResponse<MovieViewModel, string>> GetMovie(int id);

        Task<ServiceResponse<PaginatedResultSet<MovieWithReviewsViewModel>, IEnumerable<EntityError>>> GetReviewsForMovie(int id, int? page = 1, int? perPage = 5);

        Task<ServiceResponse<PaginatedResultSet<MovieViewModel>, IEnumerable<EntityError>>> FilterMoviesByDateAdded(DateTime? fromDate, DateTime? toDate, int? page = 1, int? perPage = 5);

        Task<ServiceResponse<Movie, IEnumerable<EntityError>>> PostMovie(MovieViewModel movieRequest);

        Task<ServiceResponse<Review, IEnumerable<EntityError>>> PostReviewForMovie(int movieId, ReviewViewModel reviewRequest);

        Task<ServiceResponse<Movie, IEnumerable<EntityError>>> PutMovie(int id, MovieViewModel movieRequest);

        Task<ServiceResponse<Review, IEnumerable<EntityError>>> PutReviewForMovie(int movieId, int reviewId, ReviewViewModel reviewRequest);

        Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteMovie(int id);

        Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteReviewFromMovie(int reviewId);

        bool MovieExists(int id);

        bool ReviewExists(int id);
    }
}
