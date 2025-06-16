using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;

namespace WolverineTest.Web.Handlers.Di
{
	public record DiGetUserResponse(string Id, string? Email, DateTimeOffset? LastLoginDate, DateTimeOffset? Modified);

	public static class DiGetUserEndpoint
	{
		public static DiGetUserCommand Before(string id) 
			=> new DiGetUserCommand(id);

		[WolverineGet("/di/users/{id}"), RequiresTenant]
		[Authorize]
		public static async Task<(DiGetUserResponse?, ProblemDetails)> Get(
			DiGetUserCommand command,
			IMessageBus bus
			)
		{
			var user = await bus.InvokeForTenantAsync<DiUser>(
				bus.TenantId!, command);

			if (user == null)
			{
				return (null, new ProblemDetails() { Title = "User not found", Status = 404 });
			}

			return (
				new DiGetUserResponse(user.Id, user.Email, null, null),
				WolverineContinue.NoProblems
			);
		}
	}
}