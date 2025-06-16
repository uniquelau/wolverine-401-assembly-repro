## Setup

Run `Update-Database`. 

You will need to run against each database to ensure the EF migrations are applied.

The easiest way to achieve this, is to update the 'Data' connection string in `appsettings.json`, and then run Update-Database. This will allow you to quickly target each database. Once done, discard the changes to this file.

### Other observations

Entity frameworks migrations are not applied automatically, should they? Not sure
https://jeremydmiller.com/2025/05/15/wolverine-4-is-bringing-multi-tenancy-to-ef-core/

## Issue

When the UserManager is injected into a handler, the UserManager is not scoped to the tenant database.

This results in User being created in the master tenant, rather than the tenant related to the request.

The code contains two examples:

"Explict" -> User Manager that is injected into the Endpoint is passed to a handler (this works)
"DI" -> User Manager that is injected into the handler is not scoped to the tenant

## Test results

Create_user_is_successful_explict => OK => User is created in Tenant 1.
Create_user_is_successful_di => FAIL => User does not exist in Tenant 1 (and exists in Master)

## Code-gen

### DI (UserManager loaded via method parameter)
DiCreateUserCommandHandler

```
public override async System.Threading.Tasks.Task HandleAsync(Wolverine.Runtime.MessageContext context, System.Threading.CancellationToken cancellation)
{
    using var serviceScope = _serviceScopeFactory.CreateScope();
            
    /*
    * Dependency: Descriptor: ServiceType: System.IServiceProvider Lifetime: Scoped ImplementationType: Microsoft.Extensions.DependencyInjection.ServiceDescriptor
    * Your code is directly using IServiceProvider
    */
    var userManager = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<WolverineTest.Web.Data.DbUser>>(serviceScope.ServiceProvider);
     // The actual message body
    var diCreateUserCommand = (WolverineTest.Web.Handlers.Di.DiCreateUserCommand)context.Envelope.Message;

    System.Diagnostics.Activity.Current?.SetTag("message.handler", "WolverineTest.Web.Handlers.Di.DiCreateUserHandler");
            
    // The actual message execution
    (var outgoing1, var outgoing2) = await WolverineTest.Web.Handlers.Di.DiCreateUserHandler.Handle(diCreateUserCommand, userManager).ConfigureAwait(false);
```

### Explict (UserManager passed via property on Command)
```
public override async System.Threading.Tasks.Task Handle(Microsoft.AspNetCore.Http.HttpContext httpContext)
{
    // Tenant Id detection
    // 1. Tenant Id is request header 'tenantId'
    // 2. Wolverine.Http.Runtime.MultiTenancy.FallbackDefault
    var tenantId = await TryDetectTenantId(httpContext);
    var messageContext = new Wolverine.Runtime.MessageContext(_wolverineRuntime);
    messageContext.TenantId = tenantId;
    await using var wolverineTestContext = await _dbContextBuilder.BuildAndEnrollAsync(messageContext, httpContext.RequestAborted);
    var identityErrorDescriber = new Microsoft.AspNetCore.Identity.IdentityErrorDescriber();
    using var userStore = new Microsoft.AspNetCore.Identity.EntityFrameworkCore.UserStore<WolverineTest.Web.Data.DbUser, Microsoft.AspNetCore.Identity.IdentityRole, WolverineTest.Web.Data.WolverineTestContext, string, Microsoft.AspNetCore.Identity.IdentityUserClaim<string>, Microsoft.AspNetCore.Identity.IdentityUserRole<string>, Microsoft.AspNetCore.Identity.IdentityUserLogin<string>, Microsoft.AspNetCore.Identity.IdentityUserToken<string>, Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>(wolverineTestContext, identityErrorDescriber);
    var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<WolverineTest.Web.Data.DbUser>(_options2);
    var userValidator = new Microsoft.AspNetCore.Identity.UserValidator<WolverineTest.Web.Data.DbUser>(identityErrorDescriber);
    System.Collections.Generic.IEnumerable<Microsoft.AspNetCore.Identity.IUserValidator<WolverineTest.Web.Data.DbUser>> userValidatorIEnumerable = new Microsoft.AspNetCore.Identity.IUserValidator<WolverineTest.Web.Data.DbUser>[]{userValidator};
    var passwordValidator = new Microsoft.AspNetCore.Identity.PasswordValidator<WolverineTest.Web.Data.DbUser>(identityErrorDescriber);
    System.Collections.Generic.IEnumerable<Microsoft.AspNetCore.Identity.IPasswordValidator<WolverineTest.Web.Data.DbUser>> passwordValidatorIEnumerable = new Microsoft.AspNetCore.Identity.IPasswordValidator<WolverineTest.Web.Data.DbUser>[]{passwordValidator};
    var upperInvariantLookupNormalizer = new Microsoft.AspNetCore.Identity.UpperInvariantLookupNormalizer();
    using var userManager = new Microsoft.AspNetCore.Identity.UserManager<WolverineTest.Web.Data.DbUser>(userStore, _options1, passwordHasher, userValidatorIEnumerable, passwordValidatorIEnumerable, upperInvariantLookupNormalizer, identityErrorDescriber, httpContext.RequestServices, _logger);
    Wolverine.Http.Runtime.RequestIdMiddleware.Apply(httpContext, messageContext);
    // Reading the request body via JSON deserialization
    var (exCreateUserRequest, jsonContinue) = await ReadJsonAsync<WolverineTest.Web.Handlers.Explict.ExCreateUserRequest>(httpContext);
    if (jsonContinue == Wolverine.HandlerContinuation.Stop) return;
    var exCreateUserCommand = WolverineTest.Web.Handlers.Explict.ExCreateUserEndpoint.Before(exCreateUserRequest, userManager);
            
    // The actual HTTP request handler execution
    var exCreateUserResponse_response = await WolverineTest.Web.Handlers.Explict.ExCreateUserEndpoint.Handle(exCreateUserRequest, exCreateUserCommand, messageContext).ConfigureAwait(false);
```
