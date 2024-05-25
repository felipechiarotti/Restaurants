using MediatR;
using Microsoft.Extensions.Logging;

namespace Restaurants.Application
{
    public class MediatorLoggingMiddleware<TRequest, TResponse>(ILogger<MediatorLoggingMiddleware<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("Request {@RequestType} being sent: {@Request}", typeof(TRequest).Name, request);
            var response = await next();
            logger.LogInformation("Response {@ResponseType} for the {@RequestType}: {@Response}", typeof(TResponse).Name, typeof(TRequest).Name, response);
            return response;
        }
    }
}
