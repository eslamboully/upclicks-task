using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Data.Configurations;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDailyAuthenticationsService, DailyAuthenticationsService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    
            .AddJwtBearer(options =>    
            {    
                options.TokenValidationParameters = new TokenValidationParameters    
                {    
                    ValidateIssuerSigningKey = true,    
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),    
                    ValidIssuer = _config["Jwt:Issuer"],    
                    ValidateIssuer = true,    
                    ValidateAudience = false,
                };    
            });
            services.AddDbContext<DatabaseContext>(options => {
                // configs.UseSqlite(_config.GetConnectionString("DefaultConnection"));
                // services.AddDbContext<DataContext>(options =>
                // {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    string connStr;

                    // Depending on if in development or production, use either Heroku-provided
                    // connection string, or development connection string from env var.
                    if (env == "Development")
                    {
                        // Use connection string from file.
                        connStr = _config.GetConnectionString("DefaultConnection");
                    }
                    else
                    {
                        // Use connection string provided at runtime by Heroku.
                        var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                        // Parse connection URL to connection string for Npgsql
                        connUrl = connUrl.Replace("postgres://", string.Empty);
                        var pgUserPass = connUrl.Split("@")[0];
                        var pgHostPortDb = connUrl.Split("@")[1];
                        var pgHostPort = pgHostPortDb.Split("/")[0];
                        var pgDb = pgHostPortDb.Split("/")[1];
                        var pgUser = pgUserPass.Split(":")[0];
                        var pgPass = pgUserPass.Split(":")[1];
                        var pgHost = pgHostPort.Split(":")[0];
                        var pgPort = pgHostPort.Split(":")[1];

                        connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";
                    }

                    // Whether the connection string came from the local development configuration file
                    // or from the environment variable from Heroku, use it to set up your DbContext.
                    options.UseNpgsql(connStr);
                });
            // });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
