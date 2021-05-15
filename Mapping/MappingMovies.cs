using AutoMapper;
using Lab1_.NET.Models;
using Lab1_.NET.ViewModels;

namespace Lab1_.NET.Mapping
{
    public class MappingMovies : Profile
    {
        public MappingMovies()
        {
            //CreateMap<Movie, MovieViewModel>();

            CreateMap<MovieViewModel, Movie>();
        }
    }
}
