using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;

namespace WolverineTest.Web.Handlers.Explict
{
	public record ExGetUserResponse(string Id, string? Email, DateTimeOffset? LastLoginDate, DateTimeOffset? Modified);

	public static class ExGetUserEndpoint
	{
		public static ExGetUserCommand Before(string id) 
			=> new ExGetUserCommand(id);

		[WolverineGet("/ex/users/{id}"), RequiresTenant]
		[Authorize]
		public static async Task<(ExGetUserResponse?, ProblemDetails)> Get(
			ExGetUserCommand command,
			IMessageBus bus
			)
		{
			var user = await bus.InvokeForTenantAsync<ExUser>(
				bus.TenantId!, command);

			return (
				new ExGetUserResponse(user.Id, user.Email, null, null),
				WolverineContinue.NoProblems
			);
		}
	}
}