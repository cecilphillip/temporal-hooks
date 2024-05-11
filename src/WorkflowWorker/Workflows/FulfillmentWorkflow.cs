using Temporalio.Common;
using Temporalio.Workflows;

namespace WorkflowWorker.Workflows;

[Workflow]
public class FulfillmentWorkflow
{
    readonly RetryPolicy _retryPolicy = new RetryPolicy
    {
        InitialInterval = TimeSpan.FromSeconds(1),
        MaximumInterval = TimeSpan.FromSeconds(10),
        BackoffCoefficient = 2
    };

    private string SimpleStatus { get; set; } = "Workflow not started";

    [WorkflowRun]
    public async Task RunAsync(OrderConfirmation confirmation)
    { 
        SimpleStatus = "Starting workflow";
        var activityResult = await  Workflow.ExecuteActivityAsync((FulfillmentActivities x) => x.SendOrderConfirmation(confirmation),
            new() { RetryPolicy = _retryPolicy, StartToCloseTimeout = TimeSpan.FromSeconds(10)});

        if (activityResult)
        {
            SimpleStatus = "Order confirmation sent";
            activityResult = await  Workflow.ExecuteActivityAsync((FulfillmentActivities x) => x.UpdateInventory(confirmation),
                new() { RetryPolicy = _retryPolicy, StartToCloseTimeout = TimeSpan.FromSeconds(10)});
        }

        if (activityResult)
        {
            SimpleStatus = "Inventory updated";
            activityResult = await Workflow.ExecuteActivityAsync(
                (FulfillmentActivities x) => x.ScheduleDelivery(confirmation),
                new() { RetryPolicy = _retryPolicy, StartToCloseTimeout = TimeSpan.FromSeconds(10)});
        }

        if (activityResult)
        {
            SimpleStatus = "Delivery scheduled";
        }
    }
    
    [WorkflowQuery]
    public string CurrentStatus() => SimpleStatus;
}