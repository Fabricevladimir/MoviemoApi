using FluentAssertions;
using Moviemo.API.Extensions;
using Xunit;

namespace Moviemo.UnitTests.Extensions
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("abc", "Abc")]
        [InlineData("abc def", "Abc def")]
        public void CapitalizeFirstLetterShouldRun (string value, string expected)
        {
            value.CapitalizeFirstLetter().Should().Be(expected);
        }
    }
}
