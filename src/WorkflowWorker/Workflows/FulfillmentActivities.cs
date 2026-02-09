using DotNext;
using Temporalio.Activities;

namespace WorkflowWorker.Workflows;

public record OrderConfirmation(string OrderId, string CustomerEmail);

public class FulfillmentActivities(ILogger<FulfillmentActivities> logger)
{
    [Activity]    
    public async Task<Result<bool>> SendOrderConfirmation(OrderConfirmation confirmation)
    {
        logger.LogInformation("Sending order confirmation to {Email} for order {OrderId}", confirmation.CustomerEmail, confirmation.OrderId);
        await Task.Delay(TimeSpan.FromSeconds(2));
        logger.LogInformation("Order confirmation sent");
        return Result.FromValue(true);
    }
    
    [Activity]    
    public async Task<Result<bool>> UpdateInventory(OrderConfirmation confirmation)
    {
        logger.LogInformation("Updating inventory for order {OrderId}", confirmation.OrderId);
        await Task.Delay(TimeSpan.FromSeconds(2));
        logger.LogInformation("Inventory updated");
        return Result.FromValue(true);
    }
    
    [Activity]    
    public async Task<Result<bool>> ScheduleDelivery(OrderConfirmation confirmation)
    {
        logger.LogInformation("Scheduling delivery for order {OrderId}", confirmation.OrderId);
        await Task.Delay(TimeSpan.FromSeconds(2));
        logger.LogInformation("Delivery scheduled");
        return Result.FromValue(true);
    }
}