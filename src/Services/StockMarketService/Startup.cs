using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockMarketService.Middleware;
using IdentityServer4.AccessTokenValidation;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace StockMarketService {
    public class Startup {
        private IHostingEnvironment env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }
        

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddWebsocketManager();
            
            var clientOrigins = new List<string>();
            Configuration.GetSection("ClientUrls").Bind(clientOrigins);
            
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(clientOrigins.ToArray());
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });
            
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddScoped<Commands>();
            
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            if (env.IsDevelopment())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite("Data Source=stocks.db"));
            }
            else
            {
                //services.AddDbContext<ApplicationDbContext>(options =>
                //    options.UseSqlServer(connectionString));
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite("Data Source=stocks.db"));
                
                services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = Configuration["Authority"];
                        options.RequireHttpsMetadata = false;
                        options.ApiName = Configuration["ApiName"];
                        options.ApiSecret = Configuration["ApiSecret"];
                    });
            }
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseAuthentication();
            }

            app.UseWebSockets();
            app.SetupWebsocketServer();
            
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors("default");


            app.UseAuthorization();
            
            
            

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                db.Database.Migrate();
                Seeddb.run(db);
            }
        }
    }
}