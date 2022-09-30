using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Capisol.Interview.Assessment.Context.Entities.Shouts;

public class Shout
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual Guid Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(40)]
    public string Message { get; set; } = string.Empty;

    [Required]
    public DateTime CreateDate { get; set; } = DateTime.Now;


    [Required]
    public bool IsDeleted { get; set; } = false;

    internal static void Seed(DataContext context)
    {
        if (context.ShoutSet?.SingleOrDefault(a => a.Username == "john") == null)
            context.ShoutSet?.Add(new Shout()
            {
                Username = "John",
                Message = "drives Miss daisy",
                CreateDate = DateTime.Now,
                IsDeleted = false,
            });

        if (context.ShoutSet?.SingleOrDefault(a => a.Username == "andrew") == null)
            context.ShoutSet?.Add(new Shout()
            {
                Username = "Andrew",
                Message = "walks the dog",
                CreateDate = DateTime.Now,
                IsDeleted = false,  
            });

        context.SaveChanges();
    }
}