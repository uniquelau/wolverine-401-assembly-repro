using Wolverine.Persistence;
using WolverineTest.Web.Data;

namespace WolverineTest.Web.Handlers.Di
{
	public record DiGetUserCommand(string Id) { }
	public record DiUser(string Id, string? Email); // fake domain model

	public static class GetUserHandler
	{
		public static DiUser Handle(
			DiGetUserCommand _,
			[Entity] DbUser user)
		{
			return new DiUser(user.Id, user.Email);
		}
	}
}