using System;
using System.Threading;
using Prometheus;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace ThesisApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Http("http://localhost:5044")
                .WriteTo.Elasticsearch(ConfigureElasticSink())
                .WriteTo.Console()
                .CreateLogger();
            log.Information("Hello and welcome to the monitoring dotnetcore demo!");
            log.Information(
                $"We will attempt to mimic a live system's behaviour in terms of warnings and exceptions.{Environment.NewLine}");
            const int milliSecondsToSleep = 500;
            SetupRequirements();
            //interval settings
            var loop_counter = 0;
            const int warning_interval = 20;
            const int exception_interval = 50;
            //scrape counters
            var prom_ok = Metrics.CreateCounter("prom_ok",
                "This fields indicates the transactions that were processed correctly.");
            var prom_warning = Metrics.CreateCounter("prom_warning", "This fields indicates the warning count.");
            var prom_exception = Metrics.CreateCounter("prom_exception", "This fields indicates the exception count.");
            while (true)
            {
                //main control loop
                log.Information("Transaction processed:OK");
                prom_ok.Inc(1);
                Thread.Sleep(milliSecondsToSleep);
                if (loop_counter == warning_interval)
                {
                    prom_warning.Inc(1);
                    log.Information("\nOops that was a warning  - tread carefully...\n");
                }
                else if (loop_counter == exception_interval)
                {
                    prom_exception.Inc(1);
                    log.Information("\nAlarm! call 911 - an exception has occured!\n");
                    loop_counter = 0;
                }
                loop_counter++;
            }
        }
        private static ElasticsearchSinkOptions ConfigureElasticSink()
        {
            var httpLocalhost = "http://localhost:9200";
            return new ElasticsearchSinkOptions(new Uri(httpLocalhost))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"dimko-{DateTime.UtcNow:yyyy-MM}"
            };
        }
        private static void SetupRequirements()
        {
            var metricServer = new MetricServer(port: 1234);
            metricServer.Start();
        }
    }
}
