using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;
using WolverineTest.Web.Data;

namespace WolverineTest.Web.Handlers.Explict
{
	// The user manager needs to be constructed within the WebApi
	// if an instance is called using DI inside the handler, it will not work as expected
	public record ExCreateUserCommand(string Email, string Password, UserManager<DbUser> UserManager);
	public record ExUserCreated(string Id);

	public static class ExCreateUserHandler
	{
		public static async Task<(ExUserCreated?, ProblemDetails)> Handle(
			ExCreateUserCommand command
			)
		{
			var existingUser = await command.UserManager.FindByEmailAsync(command.Email);
			if (existingUser != null) return (null, new ProblemDetails() { Title = "User already exists", Status = 400 });
			
			var user = new DbUser()
			{
				Email = command.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = command.Email,
			};

			var result = await command.UserManager.CreateAsync(user, command.Password);
			if (!result.Succeeded)
			{
				return (null, new ProblemDetails() { Title = "User creation failed! Please check user details and try again", Status = 400 });
			}

			return (
				new ExUserCreated(user.Id), 
				WolverineContinue.NoProblems
			);
		}
	}
}