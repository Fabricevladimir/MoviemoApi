using AutoMapper;
using Moviemo.API.Mappings;
using Xunit;

namespace Moviemo.UnitTests.Mappings
{
    public class MappingProfilesTest
    {
        [Fact]
        public void AutoMapperConfigurationIsValid ()
        {
            new MapperConfiguration(opt =>
                opt.AddProfile(new MappingProfiles()))
                .AssertConfigurationIsValid();
        }
    }
}
