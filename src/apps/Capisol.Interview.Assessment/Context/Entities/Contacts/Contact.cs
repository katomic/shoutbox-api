using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Capisol.Interview.Assessment.Context.Entities.Contacts;

public class Contact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual Guid Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Firstname { get; set; } = string.Empty;

    [Required]
    [MaxLength(40)]
    public string Lastname { get; set; } = string.Empty;

    [Required]
    [MaxLength(60)]
    public string Email { get; set; } = string.Empty;

    internal static void Seed(DataContext context)
    {
        if (context.ContactSet?.SingleOrDefault(a => a.Email == "john.doe@capisol.dev") == null)
            context.ContactSet?.Add(new Contact()
            {
                Email = "john.doe@capisol.dev",
                Firstname = "John",
                Lastname = "Doe",
            });

        if (context.ContactSet?.SingleOrDefault(a => a.Email == "andrew.scott@capisol.dev") == null)
            context.ContactSet?.Add(new Contact()
            {
                Email = "andrew.scott@capisol.dev",
                Firstname = "Andrew",
                Lastname = "Scott",
            });

        context.SaveChanges();
    }
}