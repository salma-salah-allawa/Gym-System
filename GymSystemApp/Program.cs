using GymSystemApp.BLL.Services.Classes;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.BLL.Utilities;
using GymSystemApp.Contexts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Classes;
using GymSystemApp.DAL.Repositories.Contracts;
using GymSystemApp.PL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace GymSystemApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymAppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            #region Entities Services
            //builder.Services.AddScoped<IPlanRepo, PlanRepo>();
            //builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IAttatchmentService, AttachmentService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            #endregion

            #region UnitOfWork And Mapper Services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(m=>m.AddProfile(new MappingProfile()));
            #endregion

            #region Security Services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //options.Password.RequiredLength = 6;
                //options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            }).AddEntityFrameworkStores<GymAppDbContext>();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            #endregion

            var app = builder.Build();
            await app.MigrationAndSeedAsync();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
