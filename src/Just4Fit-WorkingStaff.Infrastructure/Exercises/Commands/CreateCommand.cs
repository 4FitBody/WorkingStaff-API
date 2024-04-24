namespace Just4Fit_WorkingStaff.Infrastructure.Exercises.Commands;

using Just4Fit_WorkingStaff.Core.Exercises.Models;
using MediatR;

public class CreateCommand : IRequest
{
    public Exercise? Exercise { get; set; }

    public CreateCommand(Exercise? exercise) => this.Exercise = exercise;

    public CreateCommand() {}
}