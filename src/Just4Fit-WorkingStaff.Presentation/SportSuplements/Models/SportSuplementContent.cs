using Just4Fit_WorkingStaff.Core.SportSupplements.Models;

namespace Just4Fit_WorkingStaff.Presentation.SportSuplements.Models;

public class SportSupplementContent
{
    public SportSupplement? SportSupplement { get; set; }
    public string? ImageFileName { get; set; }
    public byte[]? ImageFileContent { get; set; }
}