﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.AspNetCore.Extensions;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;

namespace NMqttServer
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHostedMqttServer(builder =>
                builder
                    .WithDefaultEndpointPort(1883)
                    .WithSubscriptionInterceptor(context => GetSubscriptionInterceptorValue(context))
                    .WithApplicationMessageInterceptor(context => GetMessageInterceptorValue(context))

                ) ;

            //this adds tcp server support based on Microsoft.AspNetCore.Connections.Abstractions
            services.AddMqttConnectionHandler();

            //this adds websocket support
            services.AddMqttWebSocketServerAdapter();

        }

        private MqttApplicationMessageInterceptorContext GetMessageInterceptorValue(MqttApplicationMessageInterceptorContext context)
        {
            return context;
        }

        private MqttSubscriptionInterceptorContext GetSubscriptionInterceptorValue(MqttSubscriptionInterceptorContext context)
        {
            if (context.TopicFilter.Topic.StartsWith("test"))
            {
                context.AcceptSubscription = true;
            }
            else
            {
                context.AcceptSubscription = false;
            }
            return context;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

    }
}
