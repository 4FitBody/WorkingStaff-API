namespace Just4Fit_WorkingStaff.Infrastructure.Food.Queries;

using Just4Fit_WorkingStaff.Core.Food.Models;
using MediatR;

public class GetByIdQuery : IRequest<Food>
{
    public int Id { get; set; }

    public GetByIdQuery(int Id)
    {
        this.Id = Id;
    }

    public GetByIdQuery() { }
}
