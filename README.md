### Implict (Found no Wolverine HTTP endpoints.)

builder.Host.UseWolverine(opts =>
{
	// opts.ApplicationAssembly = typeof(Program).Assembly;
});

```
info: Wolverine.Runtime.WolverineRuntime[0]
      Exporting Open Telemetry metrics from Wolverine with name Wolverine:Wolverine.Http, version 4.0.1.0
info: Wolverine.Http.HttpGraph[0]
      Found 0 Wolverine HTTP endpoints in assemblys Wolverine.Http
warn: Wolverine.Http.HttpGraph[0]
      Found no Wolverine HTTP endpoints. If this is not expected, check the assemblies being scanned. See https://wolverine.netlify.app/guide/http/integration.html#discovery for more information
Searching 'JasperFx, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null' for commands
Searching 'Weasel.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null' for commands
Searching 'Wolverine, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null' for commands

info: Wolverine.Runtime.WolverineRuntime[0]
      Starting Wolverine messaging for application assembly Wolverine.Http, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
info: Wolverine.Runtime.WolverineRuntime[0]
      The Wolverine code generation mode is Dynamic. This is suitable for development, but you may want to opt into other options for production usage to reduce start up time and resource utilization.
info: Wolverine.Runtime.WolverineRuntime[0]
      See https://wolverine.netlify.app/guide/codegen.html for more information
info: Wolverine.Configuration.HandlerDiscovery[0]
      Searching assembly Wolverine.Http, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null for Wolverine message handlers
warn: Wolverine.Configuration.HandlerDiscovery[0]
      Wolverine found no handlers. If this is unexpected, check the assemblies that it's scanning. See https://wolverine.netlify.app/guide/handlers/discovery.html for more information
info: Wolverine.Runtime.WolverineRuntime[0]
      Wolverine assigned node id for envelope persistence is 1378646969
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7069
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5218
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\GI\source\repos\AssemblyScanningRepro\AssemblyScanningRepro.Web
```

### Explict (Found 1 Wolverine HTTP endpoints)

builder.Host.UseWolverine(opts =>
{
	opts.ApplicationAssembly = typeof(Program).Assembly;
});

```
info: Wolverine.Runtime.WolverineRuntime[0]
      Exporting Open Telemetry metrics from Wolverine with name Wolverine:Wolverine.Http, version 4.0.1.0
info: Wolverine.Http.HttpGraph[0]
      Found 1 Wolverine HTTP endpoints in assemblys AssemblyScanningRepro.Web
Searching 'JasperFx, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null' for commands
Searching 'Weasel.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null' for commands
Searching 'Wolverine, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null' for commands

info: Wolverine.Runtime.WolverineRuntime[0]
      Starting Wolverine messaging for application assembly AssemblyScanningRepro.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
info: Wolverine.Runtime.WolverineRuntime[0]
      The Wolverine code generation mode is Dynamic. This is suitable for development, but you may want to opt into other options for production usage to reduce start up time and resource utilization.
info: Wolverine.Runtime.WolverineRuntime[0]
      See https://wolverine.netlify.app/guide/codegen.html for more information
info: Wolverine.Configuration.HandlerDiscovery[0]
      Searching assembly AssemblyScanningRepro.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null for Wolverine message handlers
info: Wolverine.Runtime.WolverineRuntime[0]
      Wolverine assigned node id for envelope persistence is 1712323986
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7069
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5218
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\GI\source\repos\AssemblyScanningRepro\AssemblyScanningRepro.Web
```