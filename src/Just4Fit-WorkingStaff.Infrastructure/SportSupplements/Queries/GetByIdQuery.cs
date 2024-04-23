namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Queries;

using Just4Fit_WorkingStaff.Core.SportSupplements.Models;
using MediatR;

public class GetByIdQuery : IRequest<SportSupplement>
{
    public int? Id { get; set; }

    public GetByIdQuery(int? id)
    {
        this.Id = id;
    }

    public GetByIdQuery() { }
}