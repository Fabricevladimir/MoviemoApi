using AutoMapper;
using Moviemo.API.Models;

namespace Moviemo.API.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles ()
        {
            CreateMap<Genre, GenreResource>();

            CreateMap<Movie, MovieResource>()
                .ForMember(mr => mr.Id, opt => opt.MapFrom(m => m.Id))
                .ForMember(mr => mr.Title, opt => opt.MapFrom(m => m.Title))
                .ForMember(mr => mr.Genre, opt => opt.MapFrom(m => m.Genre.Name))
                .ForMember(mr => mr.GenreId, opt => opt.MapFrom(m => m.GenreId));


            // Reverse mapping
            CreateMap<SaveGenreResource, Genre>()
                .ForMember(genre => genre.Id, opt => opt.Ignore())
                .ForMember(genre => genre.Movies, opt => opt.Ignore());

            CreateMap<GenreResource, Genre>()
                .ForMember(genre => genre.Movies, opt => opt.Ignore());

            CreateMap<SaveMovieResource, Movie>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Genre, opt => opt.Ignore());

            CreateMap<MovieResource, Movie>()
                .ForMember(m => m.Title, opt => opt.MapFrom(mr => mr.Title))
                .ForMember(m => m.Genre, opt => opt.Ignore())
                .ForMember(m => m.GenreId, opt => opt.MapFrom(mr => mr.GenreId));
        }
    }
}
