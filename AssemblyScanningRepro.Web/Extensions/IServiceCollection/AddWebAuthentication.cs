using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace WolverineTest.Web.Extensions
{ 
	public static partial class IServiceCollectionExtensions
	{
		/// <summary>
		/// Registers authentication with the DI container.
		/// </summary>
		/// <remarks>Authentication, JWT, Token Providers, Identity Options.</remarks>
		internal static IServiceCollection AddWebAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				ConfigureJwtBearer(options, configuration);
			});

			services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(24));

			services.Configure<IdentityOptions>(opt =>
			{
				ConfigureIdentityOptions(opt);
			});

			return services;
		}

		private static void ConfigureJwtBearer(JwtBearerOptions options, IConfiguration configuration)
		{
			var jwtSecret = configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT:Secret is not defined, startup aborted.");

			options.SaveToken = true;
			options.RequireHttpsMetadata = false;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ClockSkew = TimeSpan.FromSeconds(30),
				ValidAudience = configuration["JWT:ValidAudience"],
				ValidIssuer = configuration["JWT:ValidIssuer"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
			};
			options.Events = new JwtBearerEvents
			{
				OnAuthenticationFailed = context =>
				{
					Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
					return Task.CompletedTask;
				}
			};
		}

		private static void ConfigureIdentityOptions(IdentityOptions options)
		{
			options.Password.RequireDigit = false;
			options.Password.RequireLowercase = false;
			options.Password.RequireUppercase = false;
			options.Password.RequireNonAlphanumeric = false;
		}
	}
}
