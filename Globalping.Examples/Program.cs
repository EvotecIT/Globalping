using Globalping.Examples;

BuildRequestExample.Run();
ExecuteMeasurementExample.RunAsync().GetAwaiter().GetResult();
ExecuteMtrExample.RunAsync().GetAwaiter().GetResult();
ExecuteDnsExample.RunAsync().GetAwaiter().GetResult();
ExecuteTracerouteExample.RunAsync().GetAwaiter().GetResult();
ExecuteHttpExample.RunAsync().GetAwaiter().GetResult();
