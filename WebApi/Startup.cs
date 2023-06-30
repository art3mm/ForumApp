using AutoMapper;
using BLL;
using BLL.Configuration;
using BLL.Interfaces;
using BLL.Services;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi;

namespace WebApplication1
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            ForumConfiguration.Configuration = _configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ForumDbContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("ForumDbConnection"))
            );

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }
            );

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ForumDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ITopicService, TopicService>();

            services.AddScoped<IMessageService, MessageService>();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutomapperProfile());
            }
            );

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.Configure<EmailSenderConfiguration>(_configuration.GetSection("ForumConfigurationData:EmailSender"));

            services.AddScoped<INotificationSenderService, EmailSenderService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
