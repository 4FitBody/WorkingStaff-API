namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Commands;

using Just4Fit_WorkingStaff.Core.SportSupplements.Models;
using MediatR;


public class CreateCommand : IRequest
{
    public SportSupplement? SportSupplement { get; set; }
    public CreateCommand(SportSupplement? SportSupplement) => this.SportSupplement = SportSupplement;
    public CreateCommand() {}
}
