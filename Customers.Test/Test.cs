using System.Threading.Tasks;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Customers.Test
{
    public class Test : IClassFixture<DependencySetupFixture>
    {
        private readonly ServiceProvider _serviceProvider;

        public Test(DependencySetupFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }
        
        [Fact]
        public async Task CanLogin()
        {
            using var scope = _serviceProvider.CreateScope();
            
            var authService = scope.ServiceProvider.GetService<IAuthService>();
            var authResponse = await authService.SignIn(new AuthorizationRequest
            {
                Email = "admin@app.com",
                Password = "admin@123"
            });

            Assert.Equal(200, authResponse.Code);
        }

        [Fact]
        public async Task CanLoginAndRefresh()
        {
            using var scope = _serviceProvider.CreateScope();

            //Sign in.
            var authService = scope.ServiceProvider.GetService<IAuthService>();
            var authResponse = await authService.SignIn(new AuthorizationRequest
            {
                Email = "admin@app.com",
                Password = "admin@123"
            });
            
            var auth = authResponse as AuthorizationResponse;

            Assert.Equal(200, authResponse.Code);
            Assert.NotNull(auth);

            //Refresh.
            var refreshResponse = await authService.RefreshAccessToken(new RefreshRequest
            {
                Email = "admin@app.com",
                RefreshToken = auth.RefreshToken
            });

            Assert.Equal(200, refreshResponse.Code);
        }

        [Fact]
        public async Task CanLoginAndLogout()
        {
            using var scope = _serviceProvider.CreateScope();

            //Sign in.
            var authService = scope.ServiceProvider.GetService<IAuthService>();
            var authResponse = await authService.SignIn(new AuthorizationRequest
            {
                Email = "admin@app.com",
                Password = "admin@123"
            });

            var auth = authResponse as AuthorizationResponse;

            Assert.Equal(200, authResponse.Code);
            Assert.NotNull(auth);

            //Logout.
            var logoutResponse = await authService.RevokeRefreshToken(new RefreshRequest
            {
                Email = "admin@app.com",
                RefreshToken = auth.RefreshToken
            });

            Assert.Equal(200, logoutResponse.Code);
        }

        [Fact]
        public async Task CanGetUsers()
        {
            using var scope = _serviceProvider.CreateScope();

            var userService = scope.ServiceProvider.GetService<IUserService>();
            var userResponse = await userService.GetUsers();

            Assert.Equal(200, userResponse.Code);
        }

        [Fact]
        public async Task CanGetCustomersAdmin()
        {
            using var scope = _serviceProvider.CreateScope();

            var customerService = scope.ServiceProvider.GetService<ICustomerService>();
            var customerResponse = await customerService.GetCustomers(new CustomerRequest(), "admin@app.com", true);

            Assert.Equal(200, customerResponse.Code);
        }

        [Fact]
        public async Task CanGetCustomers()
        {
            using var scope = _serviceProvider.CreateScope();

            var customerService = scope.ServiceProvider.GetService<ICustomerService>();
            var customerResponse = await customerService.GetCustomers(new CustomerRequest(), "seller1@app.com", false);

            Assert.Equal(200, customerResponse.Code);
        }
    }
}