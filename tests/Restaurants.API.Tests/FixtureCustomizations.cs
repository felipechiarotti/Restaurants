using AutoFixture;

namespace Restaurants.API.Tests
{
    public class DateOnlyFixtureCustomization : ICustomization
    {

        void ICustomization.Customize(IFixture fixture)
        {
            fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
        }
    }

    public class TimeOnlyFixtureCustomization : ICustomization
    {
        void ICustomization.Customize(IFixture fixture)
        {
            fixture.Customize<TimeOnly>(composer => composer.FromFactory<DateTime>(TimeOnly.FromDateTime));
        }
    }
}
