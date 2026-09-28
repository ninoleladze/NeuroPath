namespace NeuroPath.Core.Models.Domain.Exercises;

public class ExerciseCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public string? Color { get; set; }

    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
