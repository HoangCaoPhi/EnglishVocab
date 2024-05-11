using EnglishVocab.Identity.Contexts;
using EnglishVocab.Identity.Models;
using EnglishVocab.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class ApplicationInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeAsync()
    {
        Log.Logger = new LoggerConfiguration()
            .CreateLogger();
        try
        {
            var dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            var identityDbContext = _serviceProvider.GetRequiredService<IdentityContext>();
            identityDbContext.Database.Migrate();

            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await DefaultRoles.SeedAsync(userManager, roleManager);
            await DefaultSuperAdmin.SeedAsync(userManager, roleManager);
            await DefaultBasicUser.SeedAsync(userManager, roleManager);
            Log.Information("Finished Seeding Default Data");
            Log.Information("Application Starting");
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "An error occurred seeding the DB");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}