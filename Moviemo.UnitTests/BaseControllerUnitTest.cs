using AutoMapper;
using Moq;
using Moviemo.API.Mappings;
using Moviemo.API.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moviemo.UnitTests
{
    public class BaseControllerUnitTest
    {
        protected IMapper mapper;
        protected Mock<IGenreService> mockGenreService;

        public BaseControllerUnitTest ()
        {
            var config = new MapperConfiguration(opt => opt.AddProfile(new MappingProfiles()));
            mapper = config.CreateMapper();

            mockGenreService = new Mock<IGenreService>(MockBehavior.Loose);
        }

    }
}
