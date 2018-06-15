using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrapeLEAF.DataSyncService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IConfigurationSection Settings { get { return Configuration.GetSection("Settings"); } }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(r => 
            {
                var messageServiceOptions = new MessageServiceOptions();

                messageServiceOptions._queueServiceMapping = Settings.GetSection("rabbitmqServiceMapping").GetChildren()
                .ToDictionary(item => item["Queue"], item => item["Service"]);

                return messageServiceOptions;
            });

            services.AddSingleton(r =>
            {
                var options = new RabbitMQOptions();

                var rabbitmqSetting = Settings.GetSection("rabbitmq");

                options.HostName = rabbitmqSetting["HostName"];
                options.UserName = rabbitmqSetting["UserName"];
                options.Password = rabbitmqSetting["Password"];
                options.VirtualHost = rabbitmqSetting["VirtualHost"];
                options.TopicExchangeName = rabbitmqSetting["TopicExchangeName"];
                options.RequestedConnectionTimeout = int.Parse(rabbitmqSetting["RequestedConnectionTimeout"]);
                options.SocketReadTimeout = int.Parse(rabbitmqSetting["SocketReadTimeout"]);
                options.SocketWriteTimeout = int.Parse(rabbitmqSetting["SocketWriteTimeout"]);
                options.Port = int.Parse(rabbitmqSetting["Port"]);

                return options;
            });

            services.AddSingleton<IConsumerClientFactory, ConsumerClientFactory>(resolver =>
            {
                var rabbitmqOption = resolver.GetService<RabbitMQOptions>();

                return new ConsumerClientFactory(rabbitmqOption);
            });

            services.AddSingleton<ConsumerHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

            serviceProvider.GetService<ConsumerHandler>().Start();
        }
    }
}
