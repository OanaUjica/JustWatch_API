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

            CreateMap<Comment, CommentViewModel>().ReverseMap();

            CreateMap<Movie, MovieWithCommentsViewModel>().ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();

            CreateMap<Reservation, ReservationsForUserResponse>().ReverseMap();
        }
    }
}
