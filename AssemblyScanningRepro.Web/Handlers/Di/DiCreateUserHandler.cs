using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;
using WolverineTest.Web.Data;

namespace WolverineTest.Web.Handlers.Di
{
	// The user manager needs to be constructed within the WebApi
	// if an instance is called using DI inside the handler, it will not work as expected
	public record DiCreateUserCommand(string Email, string Password);
	public record DiUserCreated(string Id);

	public static class DiCreateUserHandler
	{
		public static async Task<(DiUserCreated?, ProblemDetails)> Handle(
			DiCreateUserCommand command,
			UserManager<DbUser> userManager
			)
		{
			var existingUser = await userManager.FindByEmailAsync(command.Email);
			if (existingUser != null) return (null, new ProblemDetails() { Title = "User already exists", Status = 400 });
			
			var user = new DbUser()
			{
				Email = command.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = command.Email,
			};

			var result = await userManager.CreateAsync(user, command.Password);
			if (!result.Succeeded)
			{
				return (null, new ProblemDetails() { Title = "User creation failed! Please check user details and try again", Status = 400 });
			}

			return (
				new DiUserCreated(user.Id), 
				WolverineContinue.NoProblems
			);
		}
	}
}