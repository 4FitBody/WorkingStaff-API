namespace Just4Fit_WorkingStaff.Infrastructure.Exercises.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.Exercises.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Exercises.Commands;
using MediatR;

public class DeleteHandler : IRequestHandler<DeleteCommand>
{
    private readonly IExerciseRepository exerciseRepository;

    public DeleteHandler(IExerciseRepository exerciseRepository) => this.exerciseRepository = exerciseRepository;

    public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        await this.exerciseRepository.DeleteAsync((int)request.Id);
    }
}