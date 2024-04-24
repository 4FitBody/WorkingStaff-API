namespace Just4Fit_WorkingStaff.Infrastructure.Exercises.Queries;

using Just4Fit_WorkingStaff.Core.Exercises.Models;
using MediatR;

public class GetByIdQuery : IRequest<Exercise>
{
    public int? Id { get; set; }

    public GetByIdQuery(int? id)
    {
        this.Id = id;
    }

    public GetByIdQuery() { }
}