using GymManagement.BLL;
using GymManagement.BLL.AttachMemnt;
using GymManagement.BLL.Class;
using GymManagement.BLL.Interface;
using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.PL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GymManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<GymManagementDbContext>(options =>
            {

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));

            builder.Services.AddScoped<IMemberServices, MemberServices>();
            builder.Services.AddScoped<IPlanServices, PlanServices>();
            builder.Services.AddScoped<ITrainerServices, TrainerServices>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<ISessionServices, SessionServices>();
            builder.Services.AddScoped<IAnalytics, Analytics>();
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>

            {
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                opt.Lockout.MaxFailedAccessAttempts = 5;




            })
                .AddEntityFrameworkStores<GymManagementDbContext>();

            var app = builder.Build();

            await app.MigrateAndSeedDatabaseAsync();





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
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
