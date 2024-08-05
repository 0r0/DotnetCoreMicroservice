using System.Reflection;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace Common.Logging;

public static class SeriLogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure => ((context, configuration) =>
    {
        configuration.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Elasticsearch(new[] {new Uri(context.Configuration["ElasticConfiguration:Uri"])}, opt =>
                {
                    opt.DataStream = new
                        DataStreamName("logs", "appLogs",
                            Assembly.GetExecutingAssembly()
                                .GetName().Name.ToLower()
                                .Replace(".", "-") + "-" +
                            context.HostingEnvironment.EnvironmentName
                                .ToLower().Replace(".", "-") + "-logs-" +
                            "" + DateTime.UtcNow.ToString("yyyy-MM"));
                    opt.BootstrapMethod = BootstrapMethod.Failure;
                },
                transport =>
                {
                    transport.Authentication(new BasicAuthentication(context.Configuration["ElasticConfiguration:UserName"],
                        context.Configuration["ElasticConfiguration:Password"]));
                }).Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
            .ReadFrom.Configuration(context.Configuration);
    });
}