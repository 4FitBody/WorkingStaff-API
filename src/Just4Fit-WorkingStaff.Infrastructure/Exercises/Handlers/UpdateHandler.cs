namespace Just4Fit_WorkingStaff.Infrastructure.Exercises.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.Exercises.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Exercises.Commands;
using MediatR;

public class UpdateHandler : IRequestHandler<UpdateCommand>
{
    private readonly IExerciseRepository exerciseRepository;

    public UpdateHandler(IExerciseRepository exerciseRepository) => this.exerciseRepository = exerciseRepository;

    public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        ArgumentNullException.ThrowIfNull(request.Exercise);

        await this.exerciseRepository.UpdateAsync((int)request.Id, request.Exercise);
    }
}