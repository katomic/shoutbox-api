namespace Capisol.Interview.Assessment.Models.ShoutModels;

public class ShoutModel
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Message { get; set; }

    public DateTime CreateDate { get; set; }

    public  bool ? IsDeleted { get; set; }

}
