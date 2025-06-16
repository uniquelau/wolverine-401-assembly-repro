using Microsoft.EntityFrameworkCore;

using JasperFx;
using Wolverine;
using Wolverine.Http;
using Wolverine.EntityFrameworkCore;
using Wolverine.SqlServer;

using WolverineTest.Web.Data;
using WolverineTest.Web.Extensions;

namespace WolverineTest.Web
{
	public class Program
	{
		public static async Task<int> Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var connectionString = builder.Configuration.GetConnectionString("Data");
			if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("Connection string not set.");

			builder.Services.AddWebApi(builder.Configuration);

			builder.Services.AddWolverineHttp();

			builder.Host.UseWolverine(opts =>
			{
				opts.Services.AddDbContextWithWolverineIntegration<WolverineTestContext>(x =>
				{
					x.UseSqlServer(builder.Configuration.GetConnectionString(connectionString));
				});

				opts.Services.AddDbContextWithWolverineManagedMultiTenancy<WolverineTestContext>((context, connectionString, tenantId) =>
				{
					context.UseSqlServer(connectionString.Value, b => b.MigrationsAssembly("WolverineTest.Web"));
				}, AutoCreate.CreateOrUpdate);

				opts.PersistMessagesWithSqlServer(connectionString)

						.RegisterStaticTenants(x =>
						{
							x.Register("Tenant1", builder.Configuration.GetConnectionString("Tenant1")!);
							x.Register("Tenant2", builder.Configuration.GetConnectionString("Tenant2")!);
						});

				// Set up Entity Framework Core as the support for Wolverine's transactional middleware
				opts.UseEntityFrameworkCoreTransactions();

				// Enrolling all local queues into the durable inbox/outbox processing
				opts.Policies.UseDurableLocalQueues();
			});

			var app = builder.Build();



			// Configure the HTTP request pipeline.
			app.UseHttpsRedirection();
			app.MapWolverineEndpoints(opts =>
			{
				// Register tenant detection
				opts.TenantId.IsRequestHeaderValue("tenantId");
				opts.TenantId.DefaultIs(StorageConstants.DefaultTenantId);
			});

			app.UseAuthorization();

			return await app.RunJasperFxCommands(args);
		}
	}
}