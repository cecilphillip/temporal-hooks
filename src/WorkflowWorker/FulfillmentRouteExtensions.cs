using Microsoft.AspNetCore.Mvc;
using Temporalio.Client;
using WorkflowWorker.Workflows;

namespace WorkflowWorker;

public static class FulfillmentRouteExtensions {
    public static IEndpointRouteBuilder MapFulfillmentRoutes(this IEndpointRouteBuilder endpoints)
    {
        // Fulfillment endpoint routes
        var group = endpoints.MapGroup("/fulfillment");
        group.MapPost("/fulfillment/run", RunWorkflow).WithName("workflow-run"); 
        group.MapGet("/fulfillment/status", WorkflowStatus).WithName("workflow-status");
        
        // Fulfillment endpoint handlers
        async Task<IResult> RunWorkflow([FromBody] OrderConfirmation confirmation, [FromServices] ITemporalClient temporalClient, [FromServices] LinkGenerator linkGenerator)
        {
            var handle = await temporalClient.StartWorkflowAsync<FulfillmentWorkflow>(x => x.RunAsync(confirmation), new() { Id = $"temporal-hooks-{Guid.NewGuid():N}", TaskQueue = "hooks" });

            var payload = new { workflowId = handle.Id, runId = handle.ResultRunId };
            var statusUrl = linkGenerator.GetPathByName("workflow-status", payload);
            return Results.Accepted(statusUrl, payload);
        }

        async Task<IResult> WorkflowStatus([FromQuery] string workflowId, [FromQuery] string runId, [FromServices] ITemporalClient temporalClient)
        {
            var handle = temporalClient.GetWorkflowHandle<FulfillmentWorkflow>(workflowId, runId);
            var status = await handle.QueryAsync(x => x.CurrentStatus());

            return Results.Ok(new { status });
        }
        return endpoints;
    }
}