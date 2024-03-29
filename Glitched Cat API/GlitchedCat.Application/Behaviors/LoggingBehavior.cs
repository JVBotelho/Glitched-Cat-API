using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GlitchedCat.Domain.Common.Logging;
using MediatR;

namespace GlitchedCat.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILoggingService _loggingService;

    public LoggingBehavior(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        
        var log = new LogData()
        {
            Log = $"Handling {typeof(TRequest).Name}",
            ExecutionTime = DateTime.Now,
        };
        
        _ = _loggingService.LogAsync(log);
        
        stopwatch.Start();

        var response = await next();
        
        stopwatch.Stop();
        
        log.Log = $"Handled {typeof(TRequest).Name}";
        log.ExecutionTime = DateTime.Now;
        log.TimeElapsed = stopwatch.Elapsed;

        _ = _loggingService.LogAsync(log);

        return response;
    }
}