using Loyalto;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Net.Http;
using WebAPI.Data;

namespace WebAPI
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
            //services.AddControllers();
            services.AddOptions(); 
            services.AddSingleton(Configuration);

            // Add LoyaltyContext
            DbContextOptions<LoyaltoContext> loyaltoContextOptions = null;
            services.AddDbContext<LoyaltoContext>(optionsBuilder =>
            {
                optionsBuilder.UseMySQL(Configuration.GetConnectionString("Loyalto"), MySQLOptionsAction: sqlOptions =>
                {
                    //sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(1200);
                });
                loyaltoContextOptions = (DbContextOptions<LoyaltoContext>)optionsBuilder.Options;
            });

            services.AddScoped<ILoyaltoDbInitializer, LoyaltoDbInitializer>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            // Initilize database
            ILoyaltoDbInitializer loyaltoDbInitializer = serviceProvider.GetService<ILoyaltoDbInitializer>();
            loyaltoDbInitializer.InitializeAsync();

            //Add MVC
            services.AddMvc(config =>
            {
                //var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //config.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(o =>
            {
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // Add DbContextOption
            services.AddSingleton(loyaltoContextOptions);

            // Add MediatR
            services.AddMediatR();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "WebAPI", Version = "v1" });
                c.CustomSchemaIds(i => i.FullName);
            });

            // LookupDataRetriever
            services.AddSingleton<ILookupDataRetriever, LookupDataRetriever>();

            // HttpClient for calling gateway
            services.AddSingleton<HttpClient>();
            services.AddHttpClient<IManagerService, ManagerService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ValutaUrl"]);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IMemoryCache memoryCache, DbContextOptions<LoyaltoContext> dbContextOptions, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Add OnMemoryLookupData into MemoryCache for faster access
            OnMemoryLookupData onMemoryLookupData = new OnMemoryLookupData(dbContextOptions);
            memoryCache.Set("OnCacheLookupData", onMemoryLookupData, DateTimeOffset.MaxValue);

            app.UseAuthentication();
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI V1");
            });
            app.UseMvc();
        }
    }
}
