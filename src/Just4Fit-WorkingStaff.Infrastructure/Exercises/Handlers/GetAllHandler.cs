namespace Just4Fit_WorkingStaff.Infrastructure.Exercises.Handlers;

using Just4Fit_WorkingStaff.Infrastructure.Exercises.Queries;
using Just4Fit_WorkingStaff.Core.Exercises.Models;
using MediatR;
using Just4Fit_WorkingStaff.Core.Exercises.Repositories;

public class GetAllHandler : IRequestHandler<GetAllQuery, IEnumerable<Exercise>>
{
    private readonly IExerciseRepository exerciseRepository;

    public GetAllHandler(IExerciseRepository exerciseRepository) => this.exerciseRepository = exerciseRepository;

    public async Task<IEnumerable<Exercise>?> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var exercises = await this.exerciseRepository.GetAllAsync();

        if (exercises is null)
        {
            return Enumerable.Empty<Exercise>();
        }

        return exercises;
    }
}