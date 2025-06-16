using Wolverine.Persistence;
using WolverineTest.Web.Data;

namespace WolverineTest.Web.Handlers.Explict
{
	public record ExGetUserCommand(string Id) { }
	public record ExUser(string Id, string? Email); // fake domain model

	public static class ExGetUserHandler
	{
		public static ExUser Handle(
			ExGetUserCommand _,
			[Entity] DbUser user)
		{
			return new ExUser(user.Id, user.Email);
		}
	}
}