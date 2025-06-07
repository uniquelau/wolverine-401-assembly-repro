using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;

namespace AssemblyScanningRepro.Web.Handlers
{
	public record CreateIssue(string Name);
	public record IssueResponse(string Name, Guid Id);

	public class IssueHandler
	{
		public static ProblemDetails Validate(CreateIssue command)
		{
			if (command.Name == "Weird")
			{
				return new ProblemDetails
				{
					Detail = "There is a problem",
					Status = 400
				};
			}

			return WolverineContinue.NoProblems;
		}

		[WolverinePost("/issues/create")]
		public IssueResponse Handle(
			CreateIssue command)
		{
			return (
				new IssueResponse(command.Name, Guid.NewGuid())
			);
		}
	}
}
