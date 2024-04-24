namespace Just4Fit_WorkingStaff.Infrastructure.Food.Commands;

using Just4Fit_WorkingStaff.Core.Food.Models;
using MediatR;

public class UpdateCommand : IRequest
{
    public int? Id { get; set; }
    public Food? Food { get; set; }

    public UpdateCommand(int? id, Food? food)
    {
        this.Id = id;

        this.Food = food;
    }

    public UpdateCommand() {}
}