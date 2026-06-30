var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AppHost_ApiService>("apiservice");

builder.AddProject<Projects.AppHost_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
