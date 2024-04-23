using MediatR;

namespace Just4Fit_WorkingStaff.Infrastructure.Exercises.Commands;

public class ApproveCommand : IRequest
{
    public int? Id { get; set;}

    public ApproveCommand(int? id)
    {
        this.Id = id;
    }

    public ApproveCommand() { }
}