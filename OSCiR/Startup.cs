using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCiR.Areas.Shared;
using OSCiR.Areas.Shared.swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OSCiR.Areas.Shared.Auth;
using OSCiR.Shared;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace OSCiR
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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions( //this fixes the error: JSON.Net Self referencing loop detected https://stackoverflow.com/questions/13510204/json-net-self-referencing-loop-detected
                    options => options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvcCore()
                .AddApiExplorer();
                

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            //services.AddScoped<IUserService, UserService>();


            var connection = Configuration.GetConnectionString("DefaultConnection"); //"Server=127.0.0.1;Port=5432;Database=oscir;User Id=oscir;Password=password;";
            services.AddDbContext<CMDbContext>(options =>
            {
                options.UseNpgsql(connection);
            });
            //Console.WriteLine("[" + connection + "]");

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "OSCiR API", Version = "v1" });

                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                 


                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                options.DocumentFilter<SecurityRequirementsDocumentFilter>();

            });

            //Add auth handlers
            services.AddTransient<IAuthorizationHandler, OwnerAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, UserAuthorizationHandler>();

            services.AddTransient<IAuthorizationHandler, ClassAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, ClassRelationshipAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, ClassExtendAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, ClassPropertyAuthorizationHandler>();

            services.AddTransient<IAuthorizationHandler, ConfigItemAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, ConfigItemRelationshipAuthorizationHandler>();

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
                //app.UseHsts();
                /*
                 * Unhandled Exception: System.InvalidOperationException: Unable to configure HTTPS endpoint. No server certificate was specified, and the default developer certificate could not be found.
To generate a developer certificate run 'dotnet dev-certs https'. To trust the certificate (Windows and macOS only) run 'dotnet dev-certs https --trust'.              
                 */
            }

            //FIXME comment out when running automated tests?
           UpdateDatabase(app); //auto apply any migrations on startup


            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "OSCiR API V1");
                //c.RoutePrefix = "docs";
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<CMDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
