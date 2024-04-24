namespace Just4Fit_WorkingStaff.Infrastructure.Exercises.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.Exercises.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Exercises.Commands;
using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand>
{
    private readonly IExerciseRepository exerciseRepository;

    public CreateHandler(IExerciseRepository exerciseRepository) => this.exerciseRepository = exerciseRepository;

    public async Task Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Exercise);

        await this.exerciseRepository.CreateAsync(request.Exercise);
    }
}