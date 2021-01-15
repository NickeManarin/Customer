using System;
using System.IO;
using System.Text;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Customers.Application.Services;
using Customers.Data.Context;
using Customers.Data.Repository;
using Customers.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Customers.Test
{
    public class DependencySetupFixture
    {
        public DependencySetupFixture()
        {
            var services = new ServiceCollection();
            services.AddDbContext<DataContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CustomerDatabase;Trusted_Connection=True;MultipleActiveResultSets=true"));

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

            services.AddCacheManager<string>(inline => inline.WithDictionaryHandle());

            //Configuration.
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, true);
            var configuration = builder.Build();
            
            //Configure strongly typed settings objects.
            var appSettingsSection = configuration.GetSection("AppSettings");
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

            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });
            
            ServiceProvider = services.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }
}
