# Temporal + AspNetCore Sample

Sample application that shows how to integrate Temporal with AspNetCore.

## Prerequisites

* [.NET SDK v8.0+](https://get.dot.net/)
* [Temporal CLI](https://docs.temporal.io/cli#install)

### Running the demo 🚀

Run the Temporal server

```shell
>  temporal server start-dev --color always --namespace TemporalHooks   
```

Start the .NET API worker

```shell
> dotnet run 
```

### [Endpoints](./src/WorkflowWorker/FulfillmentRouteExtensions.cs)

| Endpoint                                  | HTTP METHOD | Description                                                                         |
|-------------------------------------------|-------------|-------------------------------------------------------------------------------------|
| http://localhost:5000/fulfillment/run     | POST        | Start workflow endpoint. Requires a JSon payment with `orderId` and `customerEmail` |
| http://localhost:5000//fulfillment/status | GET         | Status endpoint. Requires a `workflowId` and `runID` via query string               |

