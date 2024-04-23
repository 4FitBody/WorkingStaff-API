namespace Just4Fit_WorkingStaff.Infrastructure.News.Queries;

using Just4Fit_WorkingStaff.Core.News.Models;
using MediatR;

public class GetAllQuery : IRequest<IEnumerable<News>>
{

}