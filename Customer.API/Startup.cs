using System;
using System.Text;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Customers.Application.Services;
using Customers.Data.Context;
using Customers.Data.Repository;
using Customers.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VueCliMiddleware;

namespace Customer.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            //Application services.
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGenderService, GenderService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<ICityService, CityService>();
            services.AddTransient<IClassificationService, ClassificationService>();
            services.AddTransient<ICustomerService, CustomerService>();

            //Data.
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            
            services.AddDbContext<DataContext>(o =>
            {
                o.UseSqlServer(Configuration.GetConnectionString("CustomerConnection"));
            });
            
            services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });

            services.AddCacheManager<string>(inline => inline.WithDictionaryHandle());

            //Configure strongly typed settings objects.
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            //Configure JWT authentication.
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(1),
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Global Cors policy.
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = env.IsDevelopment() ? "ClientApp" : "dist";

                if (env.IsDevelopment())
                    spa.UseVueCli(npmScript: "serve");
            });

            //Update database to the latest version.
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.Migrate();
        }
    }
}