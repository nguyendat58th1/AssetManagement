using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using BackEndAPI.DBContext;
using BackEndAPI.Filters;
using BackEndAPI.Helpers;
using BackEndAPI.Interfaces;
using BackEndAPI.Entities;
using BackEndAPI.Repositories;
using BackEndAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BackEndAPI
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
            
            services.AddCors(options =>
              {
                  options.AddDefaultPolicy(
                  builder =>
                  {
                      builder.WithOrigins("https://localhost:5001",
                                    "http://localhost:3000")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials();
                  });
              });

            services.AddDbContext<AssetsManagementDBContext>(
              opts => opts.UseLazyLoadingProxies()
                          .UseSqlServer(Configuration.GetConnectionString("SqlConnection")));

            services.AddCors();

            services.AddControllers()
              .AddNewtonsoftJson(
                opts => opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
              );
            services.AddControllers(opts =>
            {
                opts.Filters.Add(typeof(CustomExceptionFilter));
            });
            services.AddControllers();

            // configure settings object
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                authBuilder =>
                {
                    authBuilder.RequireRole("Admin");
                });
                options.AddPolicy("User",
                authBuilder =>
                {
                    authBuilder.RequireRole("User");
                });
            });

            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddTransient<IAsyncUserRepository, UserRepository>();
            services.AddTransient<IAsyncAssignmentRepository, AssignmentRepository>();
            services.AddTransient<IAsyncReturnRequestRepository, ReturnRequestRepository>();
            services.AddTransient<IAsyncAssetCategoryRepository, AssetCategoryRepository>();
            services.AddTransient<IAsyncAssetRepository, AssetRepository>();

            services.AddScoped<IReturnRequestService, ReturnRequestService>();            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAssetCategoryService, AssetCategoryService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IReportService, ReportService>();

            services.AddIdentity<User, Role>()
                             .AddEntityFrameworkStores<AssetsManagementDBContext>()
                             .AddDefaultTokenProviders();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "back_end", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "back_end v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
