namespace Just4Fit_WorkingStaff.Infrastructure.Exercises.Handlers;

using Just4Fit_WorkingStaff.Core.Exercises.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Exercises.Commands;
using MediatR;

public class ApproveHandler : IRequestHandler<ApproveCommand>
{
    private readonly IExerciseRepository exerciseRepository;

    public ApproveHandler(IExerciseRepository exerciseRepository) => this.exerciseRepository = exerciseRepository;

    public async Task Handle(ApproveCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        await this.exerciseRepository.ApproveAsync((int)request.Id);
    }
}