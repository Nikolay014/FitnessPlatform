using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreArchTemplate.Data;
namespace AspNetCoreArchTemplate.Web
{
    using Data;
    using FitnessPlatform.Data.Models;
    using FitnessPlatform.Data.Seeding;
    using FitnessPlatform.Services.Core;
    using FitnessPlatform.Services.Core.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
            
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services
                .AddDbContext<FitnessDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            builder.Services.AddScoped<IGymService, GymService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IDailyLogService, DailyLogService>();
            builder.Services.AddScoped<IWorkoutService, WorkoutService>();
            builder.Services.AddScoped<IFoodService, FoodService>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<FitnessDbContext>()
                .AddDefaultTokenProviders();;
            builder.Services.AddControllersWithViews();

            WebApplication? app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await DataSeeder.SeedRolesAndAdminAsync(services);
                await DataSeeder.SeedSubscriptionPlansAsync(services);
                await DataSeeder.SeedSpecialtiesAsync(services);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
