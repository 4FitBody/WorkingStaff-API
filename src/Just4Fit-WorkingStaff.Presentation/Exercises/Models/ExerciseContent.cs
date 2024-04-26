using Just4Fit_WorkingStaff.Core.Exercises.Models;

namespace Just4Fit_WorkingStaff.Presentation.Exercises.Models;

public class ExerciseContent
{
    public Exercise? Exercise { get; set; }
    public string? ImageFileName { get; set; }
    public byte[]? ImageFileContent { get; set; }
}