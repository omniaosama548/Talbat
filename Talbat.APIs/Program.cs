using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talbat.APIs.Errors;
using Talbat.APIs.Extensions;
using Talbat.APIs.Helpers;
using Talbat.APIs.MiddleWares;
using Talbat.Core.Entites.Identity;
using Talbat.Core.Repositories;
using Talbat.Repository;
using Talbat.Repository.Data;
using Talbat.Repository.Identity;

namespace Talbat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Configure Services

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            
            builder.Services.AddAppServices();//from class ApplicationServerExtension
            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });
            
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.WithOrigins(builder.Configuration["FrontBaseURL"]);
                });
            });
                #endregion


                var app = builder.Build();

            #region updateDataBase
            using var Scope=app.Services.CreateScope();
            //group of services that lifeTime Scope
            var Services = Scope.ServiceProvider;
            // Servise itself
            var loggerFactory=Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var DbContext = Services.GetRequiredService<StoreContext>();
                //Ask CLR for creating object from StoreContext Explicitly
                await DbContext.Database.MigrateAsync();

                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();
                var userManager = Services.GetRequiredService<UserManager<AppUser>>();
                
                //data seed
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);
               await StoreContextSeed.SeedAsync(DbContext);
            }
            catch (Exception ex) { 
                var logger=loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occoured about Applying Migration");
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExpectionMiddleWare>();

                app.AddSwagger();
            }
            app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
