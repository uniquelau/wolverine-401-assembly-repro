using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WolverineTest.Web.Data
{
	public partial class WolverineTestContext : IdentityDbContext<DbUser>
	{
		#region DbSets

		#endregion

		public WolverineTestContext(DbContextOptions<WolverineTestContext> options)
			: base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			OnModelCreatingPartial(modelBuilder);
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<IdentityRole>();

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
