namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Commands;

using Just4Fit_WorkingStaff.Core.SportSupplements.Models;
using MediatR;

public class UpdateCommand : IRequest
{
    public int? Id { get; set; }
    public SportSupplement? SportSupplement { get; set; }

    public UpdateCommand(int? id, SportSupplement? SportSupplement)
    {
        this.Id = id;

        this.SportSupplement = SportSupplement;
    }

    public UpdateCommand() {}
}