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
        Task<ServiceResponse<List<MovieViewModel>, IEnumerable<EntityError>>> GetMovies();

        Task<ServiceResponse<MovieViewModel, string>> GetMovie(int id);

        Task<ServiceResponse<List<MovieWithCommentsViewModel>, IEnumerable<EntityError>>> GetCommentsForMovie(int id);

        Task<ServiceResponse<List<MovieViewModel>, IEnumerable<EntityError>>> FilterMoviesByDateAdded(DateTime? fromDate, DateTime? toDate);

        Task<ServiceResponse<Movie, IEnumerable<EntityError>>> PostMovie(MovieViewModel movieRequest);

        Task<ServiceResponse<Comment, IEnumerable<EntityError>>> PostCommentForMovie(int movieId, CommentViewModel commentRequest);

        Task<ServiceResponse<Movie, IEnumerable<EntityError>>> PutMovie(int id, MovieViewModel movieRequest);

        Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteMovie(int id);

        bool MovieExists(int id);
    }
}
