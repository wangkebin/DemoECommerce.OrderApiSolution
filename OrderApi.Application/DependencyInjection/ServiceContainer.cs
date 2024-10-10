using eCommerce.SharedLibrary.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;

namespace OrderApi.Application.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services,
        IConfiguration config)
    {
        //register httpclient
        // create DI
        services.AddHttpClient<IOrderService, OrderService>(options =>
        {
            options.BaseAddress = new Uri(config.GetValue<string>("OrderApiUrl"));
            options.Timeout = TimeSpan.FromSeconds(1);
        });
        //retry strategy
        var retryStrtegy = new RetryStrategyOptions()
        {
            ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
            BackoffType = DelayBackoffType.Constant,
            MaxRetryAttempts = 3,
            UseJitter = true,
            Delay = TimeSpan.FromMilliseconds(500),
            OnRetry = args =>
            {
                string message = $"OnRetry, attempt: {args.AttemptNumber} outcome {args.Outcome}";
                LogException.LogToConsole(message);
                LogException.LogToDebugger(message);
                return ValueTask.CompletedTask;
            }
        };
        
        // user retry strategy
        services.AddResiliencePipeline("my-retry-pipeline", builder =>
        {
            builder.AddRetry(retryStrtegy);
        });

        return services;
    }
}