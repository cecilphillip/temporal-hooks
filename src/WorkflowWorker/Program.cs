using Temporalio.Extensions.Hosting;
using WorkflowWorker;
using WorkflowWorker.Workflows;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedTemporalWorker("localhost:7233", "TemporalHooks", "hooks")
    .AddWorkflow<FulfillmentWorkflow>()
    .AddTransientActivities<FulfillmentActivities>();

builder.Services.AddTemporalClient("localhost:7233", "TemporalHooks");

var app = builder.Build();
app.MapFulfillmentRoutes();

await app.RunAsync();