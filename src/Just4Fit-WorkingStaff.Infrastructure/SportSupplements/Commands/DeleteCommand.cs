namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Commands;

using MediatR;

public class DeleteCommand : IRequest
{
    public int? Id { get; set; }
    public DeleteCommand(int? id) => this.Id = id;
    public DeleteCommand() { }
}
