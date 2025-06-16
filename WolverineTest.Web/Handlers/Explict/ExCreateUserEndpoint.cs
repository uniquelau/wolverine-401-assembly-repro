using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Wolverine;
using Wolverine.Http;
using WolverineTest.Web.Data;

namespace WolverineTest.Web.Handlers.Explict
{
	public record ExCreateUserRequest(string Email, string Password);
	public record ExCreateUserResponse(string Id); // would normally return the full user to web api consumer

	public static class ExCreateUserEndpoint
	{
		public static ExCreateUserCommand Before(ExCreateUserRequest request, UserManager<DbUser> userManager)
			=> new ExCreateUserCommand(request.Email, request.Password, userManager);

		[WolverinePost("/ex/users"), RequiresTenant]
		[Authorize]
		public static async Task<ExCreateUserResponse> Handle(
			ExCreateUserRequest _,
			ExCreateUserCommand command,
			IMessageBus bus)
		{
			var response = await bus.InvokeForTenantAsync<ExUserCreated>(bus.TenantId!, command);
			return new ExCreateUserResponse(response.Id);
		}
	}
}