using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace MicrosoftEntraIdDemoApp.Logic.UnitTests.Fixtures
{
    public abstract class TestBase<TInstance>
    {
        protected IFixture Fixture { get; init; }

        public TestBase()
        {
            Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        public abstract TInstance CreateFixure();
    }
}
