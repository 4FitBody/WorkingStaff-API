namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Queries;

using Just4Fit_WorkingStaff.Core.SportSupplements.Models;
using MediatR;


public class GetAllQuery : IRequest<IEnumerable<SportSupplement>>
{

}
