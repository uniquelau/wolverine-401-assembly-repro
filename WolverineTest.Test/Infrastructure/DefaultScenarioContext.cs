using Alba;

namespace WolverineTest.Test.Infrastructure
{
	[Collection("default")]
	public abstract class DefaultScenarioContext 
	{
		protected DefaultScenarioContext(WebAppFixture fixture)
		{
			Host = fixture.Host;
		}

		public IAlbaHost Host { get; }
	}
}
