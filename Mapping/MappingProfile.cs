using AutoMapper;
using Lab1_.NET.Models;
using Lab1_.NET.ViewModels;
using Lab1_.NET.ViewModels.Reservations;

namespace Lab1_.NET.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieViewModel>().ReverseMap();

            CreateMap<Review, ReviewViewModel>().ReverseMap();

            CreateMap<Movie, MovieWithReviewsViewModel>().ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();

            CreateMap<Watchlist, WatchlistsForUserResponse>().ReverseMap();
        }
    }
}
