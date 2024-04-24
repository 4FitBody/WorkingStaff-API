namespace Just4Fit_WorkingStaff.Infrastructure.News.Queries;

using Just4Fit_WorkingStaff.Core.News.Models;
using MediatR;

public class GetByIdQuery : IRequest<News>
{
    public int? Id { get; set; }

    public GetByIdQuery(int? id)
    {
        this.Id = id;
    }

    public GetByIdQuery() { }
}