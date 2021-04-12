using Microsoft.Extensions.DependencyInjection;
using System;
using MassTransit;
using GreenPipes;

namespace Financial.Chat.Web.API.Config
{
    public static class MassTransitSetup
    {
        public static void AddMassTransitSetup(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddRabbitMqMessageScheduler();

                x.AddConsumersFromNamespaceContaining<Worker>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.UseDelayedExchangeMessageScheduler();

                    cfg.ReceiveEndpoint("message_chat", e =>
                    {
                        //var retryIntervals = TimeSpanExtensions.GetIntervalsFromString("1m, 5m, 30m, 1h, 6h, 12h, 1d");
                        //e.UseScheduledRedelivery(r => r.Intervals(retryIntervals));
                        e.DiscardFaultedMessages();

                        e.ConfigureConsumer<Consumer.MessageConsumer>(context);
                        e.ConfigureConsumer<Consumer.MessageFaultConsumer>(context);
                    });
                });
            });
        }
    }
}
