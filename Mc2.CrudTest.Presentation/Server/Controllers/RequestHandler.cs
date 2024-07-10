using Mc2.CrudTest.Presentation.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Mc2.CrudTest.Presentation.Server.Controllers;
public static class RequestHandler
{
    public static async Task<IActionResult> HandleCommand<T>(
        T request, IMediator mediator, ILogger log)
    {
        try
        {
            log.Debug("Handling HTTP request of type {type}", typeof(T).Name);
            await mediator.Send(request);
            return new OkResult();
        }
        catch (Exception e)
        {
            log.Error(e, "Error handling the command");
            return ReturnError(e);
        }
    }


    public static async Task<IActionResult> HandleQuery<TModel>(
        TModel query, IMediator mediator, ILogger log)
    {
        try
        {
            log.Debug("Handling HTTP request of type {type}", typeof(TModel).Name);
            return new OkObjectResult(await mediator.Send(query));
        }
        catch (Exception e)
        {
            log.Error(e, "Error handling the query");
            return ReturnError(e);
        }
    }

    private static IActionResult ReturnError(Exception e)
    {
#if DEBUG
        return new BadRequestObjectResult(new
        {
            errorCode = e.Message,
            
           // stackTrace = e.StackTrace
        });
#else
            return new BadRequestObjectResult(new
            {
                error = e.Message
            });
#endif
    }
}