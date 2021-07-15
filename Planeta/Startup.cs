using Core.Repositories.Subnet;
using Core.Repositories.User;
using Core.Services.Subnet;
using Core.Services.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Planeta.Extentions;

namespace Planeta
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Planeta", Version = "v1" });
            });

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IUserRepository, UserRepository>(provider => new UserRepository(connectionString));
            services.AddTransient<ISubnetRepository, SubnetRepository>(provider => new SubnetRepository(connectionString));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISubnetService, SubnetService>();

            services.InitializeDataBase(connectionString);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlanetaApi"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
