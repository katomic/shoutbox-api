using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Capisol.Interview.Assessment.Context.Entities.Companies;

public class Company
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    [Required]
   
    public bool Active { get; set; } = true;

    internal static void Seed(DataContext context)
    {
        if (context.CompanySet?.SingleOrDefault(a => a.Name == "Example Company 1") == null)
            context.CompanySet?.Add(new Company()
            {
                Name = "Example Company 1",
                RegistrationDate = DateTime.Now.AddMonths(-240),
                Active = true,
            });

        if (context.CompanySet?.SingleOrDefault(a => a.Name == "Example Company 2") == null)
            context.CompanySet?.Add(new Company()
            {
                Name = "Example Company 2",
                RegistrationDate = DateTime.Now.AddMonths(-180),
                Active = true,
            });

        if (context.CompanySet?.SingleOrDefault(a => a.Name == "Example Company 3") == null)
            context.CompanySet?.Add(new Company()
            {
                Name = "Example Company 3",
                RegistrationDate = DateTime.Now.AddMonths(-120),
                Active = true,
            });

        if (context.CompanySet?.SingleOrDefault(a => a.Name == "Example Company 4") == null)
            context.CompanySet?.Add(new Company()
            {
                Name = "Example Company 4",
                RegistrationDate = DateTime.Now.AddMonths(-90),
                Active = false,
            });

        if (context.CompanySet?.SingleOrDefault(a => a.Name == "Example Company 5") == null)
            context.CompanySet?.Add(new Company()
            {
                Name = "Example Company 5",
                RegistrationDate = DateTime.Now.AddMonths(-60),
                Active = false,
            });

        context.SaveChanges();
    }
}