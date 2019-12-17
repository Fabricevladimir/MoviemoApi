using AutoMapper;
using Moq;
using Moviemo.API.Mappings;
using Moviemo.API.Models;
using Moviemo.API.Services;
using Moviemo.IntegrationTests.Extensions;
using System;
using System.Collections.Generic;

namespace Moviemo.UnitTests
{
    public class BaseControllerUnitTest
    {
        protected IMapper mapper;

        public BaseControllerUnitTest ()
        {
            var config = new MapperConfiguration(opt => opt.AddProfile(new MappingProfiles()));
            mapper = config.CreateMapper();
        }
    }
}
