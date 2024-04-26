namespace Just4Fit_WorkingStaff.Presentation.Food.Models;

using Just4Fit_WorkingStaff.Core.Food.Models;

public class FoodContent
{
    public Food? Food { get; set; }
    public string? ImageFileName { get; set; }
    public byte[]? ImageFileContent { get; set; }
    public string? VideoFileName { get; set; }
    public byte[]? VideoFileContent { get; set; }
}