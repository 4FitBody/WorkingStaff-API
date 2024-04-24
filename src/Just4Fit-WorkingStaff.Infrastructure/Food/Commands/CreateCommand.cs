namespace Just4Fit_WorkingStaff.Infrastructure.Food.Commands;

using Just4Fit_WorkingStaff.Core.Food.Models;
using MediatR;

public class CreateCommand : IRequest
{
    public Food? Food { get; set; }

    public CreateCommand(Food? food) => this.Food = food;

    public CreateCommand() {}
}