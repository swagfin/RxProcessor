using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RxProcessor.Extensions;
using RxProcessor.Models;
using RxProcessor.ObservableProviders;
using RxProcessor.ObservableProviders.Implementation;
using RxProcessor.ObservableSubscribers;
using RxProcessor.Processors;

namespace RxProcessor
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

            //Add my Content Providers as (Singletons)
            services.AddSingleton<ICarsObservableProvider, CarsObservableProvider>();
            services.AddSingleton<IProcessorInitializable>(svc => svc.GetRequiredService<ICarsObservableProvider>()); //We need to register IProcessorInitializable to allow it to be included in Initialization
            services.AddTransient<IProcessorInitializable,RegisteredCarsProcessor>();
            //Add my Subscribers as (Singletons)
            services.AddSingleton<IProcessorInitializable, TestCarSubscriber>();
            services.AddSingleton<IProcessorInitializable, YetAnotherCarSubscriber>();
            //For all the Above to be Initialized/Booted Up call app.UseProcessInitializables();

            services.Configure<ConfigOptions>(Configuration.GetSection("ConfigOptions"));
            services.AddControllers();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Reactive Processor", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Initializables
            app.UseProcessInitializables();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rx Processor - v1"));
            }

            //disabling for now app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}
