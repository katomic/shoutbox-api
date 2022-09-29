using Capisol.Interview.Assessment.Context.Entities.Contacts;
using Capisol.Interview.Assessment.Context.Entities.Companies;
using Microsoft.EntityFrameworkCore;

namespace Capisol.Interview.Assessment.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
       : base(options)
    {
    }

    public DbSet<Contact>? ContactSet { get; set; }

    public DbSet<Company>? CompanySet { get; set; }

    public static void Migrate(IServiceScope scope)
    {
        using var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        var newlyCreated = context.Database.EnsureCreated();
        context.Database.Migrate();

        if (newlyCreated)
        {
            Contact.Seed(context);
            Company.Seed(context);
        }
    }
}