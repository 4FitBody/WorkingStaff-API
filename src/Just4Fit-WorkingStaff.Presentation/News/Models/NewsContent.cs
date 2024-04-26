namespace Just4Fit_WorkingStaff.Presentation.News.Models;

using Just4Fit_WorkingStaff.Core.News.Models;

public class NewsContent
{
    public News? News { get; set; }
    public string? ImageFileName { get; set; }
    public byte[]? ImageFileContent { get; set; }
}