using Microsoft.AspNetCore.Authorization;
using Wolverine;
using Wolverine.Http;

namespace WolverineTest.Web.Handlers.Di
{
	public record DiCreateUserRequest(string Email, string Password);
	public record DiCreateUserResponse(string Id); // would normally return the full user to web api consumer

	public static class DiCreateUserEndpoint
	{
		public static DiCreateUserCommand Before(DiCreateUserRequest request)
			=> new DiCreateUserCommand(request.Email, request.Password);

		[WolverinePost("/di/users"), RequiresTenant]
		[Authorize]
		public static async Task<DiCreateUserResponse> Handle(
			DiCreateUserRequest _,
			DiCreateUserCommand command,
			IMessageBus bus)
		{
			var response = await bus.InvokeForTenantAsync<DiUserCreated>(bus.TenantId!, command);
			return new DiCreateUserResponse(response.Id);
		}
	}
}