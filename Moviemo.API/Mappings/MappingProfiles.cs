using AutoMapper;
using Moviemo.API.Models;

namespace Moviemo.API.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles ()
        {
            // Model to DTO
            CreateMap<Genre, GenreResource>();

            // Reverse mapping
            CreateMap<SaveGenreResource, Genre>()
                .ForMember(genre => genre.Movies, opt => opt.Ignore())
                .ForMember(genre => genre.Id, opt => opt.Ignore());

            CreateMap<GenreResource, Genre>()
                .ForMember(genre => genre.Movies, opt => opt.Ignore());
        }
    }
}
