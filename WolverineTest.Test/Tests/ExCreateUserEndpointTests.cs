using Alba;
using WolverineTest.Test.Infrastructure;
using WolverineTest.Web.Handlers.Explict;

namespace WolverineTest.Test.Tests
{
	public class ExCreateUserEndpointTest(WebAppFixture fixture) : DefaultScenarioContext(fixture)
	{
		private readonly IAlbaHost _host = fixture.Host;

		[Fact]
		public async Task Create_user_is_successful_explict()
		{
			// sut
			var request = new ExCreateUserRequest
			(
				"Created_" + Guid.NewGuid().ToString() + "@test.com",
				"Password123!"
			);

			var result = await _host.Scenario(_ =>
			{
				_.WithRequestHeader("tenantId", "Tenant1");
				_.Post.Url("/ex/users");
				_.Post.Json(request);
			});

			var response = await result.ReadAsJsonAsync<ExCreateUserResponse>();
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
