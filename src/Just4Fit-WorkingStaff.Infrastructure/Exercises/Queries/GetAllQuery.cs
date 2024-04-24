namespace Just4Fit_WorkingStaff.Infrastructure.Exercises.Queries;

using Just4Fit_WorkingStaff.Core.Exercises.Models;
using MediatR;

public class GetAllQuery : IRequest<IEnumerable<Exercise>>
{

}