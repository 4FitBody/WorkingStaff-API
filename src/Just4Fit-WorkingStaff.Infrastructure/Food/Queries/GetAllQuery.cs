namespace Just4Fit_WorkingStaff.Infrastructure.Food.Queries;

using Just4Fit_WorkingStaff.Core.Food.Models;
using MediatR;

public class GetAllQuery : IRequest<IEnumerable<Food>>
{

}