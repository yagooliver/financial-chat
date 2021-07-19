using Financial.Chat.Application.AutoMapper;
using Financial.Chat.Domain.Shared.Entity;
using Financial.Chat.Infra.Data.Context;
using Financial.Chat.Infra.Identity.Config;
using Financial.Chat.Infra.Ioc;
using Financial.Chat.Web.API.Config;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Financial.Chat.Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build());
            });

            services.AddDbContext<FinancialChatContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("FinancialChatConnection")));
                //(options => options.UseSqlServer("Server=localhost,1433;Database=financial;User=sa;Password=dev@1234"));

            services.AddIdentitySetup(Configuration);

            AutoMapperConfig.RegisterMappings();

            services.AddSwaggerSetup();

            services.AddSingleton(AutoMapperConfig.RegisterMappings().CreateMapper());

            services.AddMvc();
            services.AddLogging();

            services.AddHttpClient("FinancialChat", cfg => { cfg.Timeout = TimeSpan.FromSeconds(60); });

            services.AddHttpContextAccessor();

            services.AddMediatR(typeof(Startup));
            services.Configure<RabbitMqOptions>(options => Configuration.GetSection("RabbitMqConfig").Bind(options));
            services.AddMassTransitSetup(Configuration.GetSection("RabbitMqConfig").Get<RabbitMqOptions>());

            DependencyInjectionResolver.RegisterServices(services);

            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(15);
                //hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseGlobalExceptionMiddleware();
            app.UseSwaggerSetup();

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.EnsureMigrationOfContext<FinancialChatContext>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<Application.SignalR.ChatHub>("/chatHub", options =>
                {
                    options.TransportMaxBufferSize = 36000;
                    options.ApplicationMaxBufferSize = 36000;
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
                });
            });
        }
    }
}
