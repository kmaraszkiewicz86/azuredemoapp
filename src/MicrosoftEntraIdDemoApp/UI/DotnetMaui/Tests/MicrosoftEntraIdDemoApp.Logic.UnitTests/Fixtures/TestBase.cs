using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace MicrosoftEntraIdDemoApp.Logic.UnitTests.Fixtures
{
    public abstract class TestBase<TInstance>
    {
        public IFixture Fixture { get; init; }

        public TestBase()
        {
            Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        public TInstance CreateFixure() => Fixture.Create<TInstance>();
    }
}
