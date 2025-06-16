using Alba;
using WolverineTest.Test.Infrastructure;
using WolverineTest.Web.Handlers.Di;

namespace WolverineTest.Test.Tests
{
	public class DiCreateUserEndpointTests(WebAppFixture fixture) : DefaultScenarioContext(fixture)
	{
		private readonly IAlbaHost _host = fixture.Host;

		[Fact]
		public async Task Create_user_is_successful_di()
		{
			// sut
			var request = new DiCreateUserRequest
			(
				"Created_" + Guid.NewGuid().ToString() + "@test.com",
				"Password123!"
			);

			var result = await _host.Scenario(_ =>
			{
				_.WithRequestHeader("tenantId", "Tenant1");
				_.Post.Url("/di/users");
				_.Post.Json(request);
			});

			var response = await result.ReadAsJsonAsync<DiCreateUserResponse>();
			Assert.NotNull(response.Id);

			// checksums

			var asserUserDoesNotExistInMasterTenant = await _host.Scenario(_ =>
			{
				_.Get.Url($"/di/users/{response.Id}");
				_.StatusCodeShouldBe(404);
			});

			var assertUserExistsInTenant = await _host.Scenario(_ =>
			{
				_.WithRequestHeader("tenantId", "Tenant1");
				_.Get.Url($"/di/users/{response.Id}");
				_.StatusCodeShouldBe(200);
			});
		}
	}
}
