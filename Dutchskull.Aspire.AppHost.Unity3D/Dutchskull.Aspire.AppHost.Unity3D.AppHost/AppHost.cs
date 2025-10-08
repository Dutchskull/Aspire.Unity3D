using Dutchskull.Aspire.Unity3D.Hosting;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ProjectResource> api = builder
    .AddProject<Dutchskull_Aspire_Apphost_Unity3D_Api>("api");

IResourceBuilder<UnityProjectResource> unity = builder
    .AddUnityProject("game", "..\\..\\AspireIntegration", 1, customUnityInstallRoot: "E:\\Unity")
    .WithEnvironment("Test", "Value")
    .WithReference(api)
    .WaitFor(api);

builder
    .AddContainer("test", "docker/welcome-to-docker")
    .WaitFor(unity);

builder.Build().Run();