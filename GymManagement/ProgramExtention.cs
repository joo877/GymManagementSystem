using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.DataSessding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.PL
{
    public static class ProgramExtention
    {

        public static async Task MigrateAndSeedDatabaseAsync( this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymManagementDbContext>();
            var roleManeger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManeger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var pindingMigirations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pindingMigirations.Any()) 
            {
                logger.LogInformation($"Applying {pindingMigirations.Count()} pinding migiration");
                await dbContext.Database.MigrateAsync();
            }
            //D:\.Net Ass\programming tasks\GymManagement\GymManagement\wwwroot\Files\
            //D:\.Net Ass\programming tasks\GymManagement\GymManagement\GymManagement.PL.csproj
            var seedFolderpath = Path.Combine(app.Environment.ContentRootPath,"WWWroot","Files");
          await GymDataSeeding.SeedAsync(dbContext, seedFolderpath, logger);
         await   IdentityDataSeeding.SeedIdentityDataAsync(userManeger, roleManeger, logger);
        
        }
           
        

    }
}
