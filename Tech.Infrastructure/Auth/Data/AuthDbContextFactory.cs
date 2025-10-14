using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Tech.Infrastructure.Auth.Data;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var basePath = FindSolutionRoot(Directory.GetCurrentDirectory());

        var apiPath = Path.Combine(basePath, "Tech.API");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new AuthDbContext(optionsBuilder.Options);
    }

    private static string FindSolutionRoot(string currentPath)
    {
        var directory = new DirectoryInfo(currentPath);
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        return directory?.FullName ?? throw new InvalidOperationException("Could not find solution root directory.");
    }
}