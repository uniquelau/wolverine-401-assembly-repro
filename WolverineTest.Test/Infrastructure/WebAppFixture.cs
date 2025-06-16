using Alba;
using Alba.Security;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using WolverineTest.Web;

namespace WolverineTest.Test.Infrastructure
{
	public class WebAppFixture : IAsyncLifetime
	{
		public IAlbaHost Host { get; private set; } = null!;

		public async Task InitializeAsync()
		{
			var jwtSecurityStub = new JwtSecurityStub()
				.With(JwtRegisteredClaimNames.Email, "user@company.com")
				.With(JwtRegisteredClaimNames.Iss, "https://localhost");

			Host = await AlbaHost.For<Program>(jwtSecurityStub);
		}

		public async Task DisposeAsync()
		{
			// Gracefully dispose of the testing instance. 
			await Host.StopAsync();
		}
	}
}
