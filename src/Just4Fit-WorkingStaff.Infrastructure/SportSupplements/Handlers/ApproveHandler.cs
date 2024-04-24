namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Handlers;

using Just4Fit_WorkingStaff.Core.SportSupplements.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Commands;
using MediatR;

public class ApproveHandler : IRequestHandler<ApproveCommand>
{
    private readonly ISportSupplementRepository sportSupplementRepository;

    public ApproveHandler(ISportSupplementRepository sportSupplementRepository) => this.sportSupplementRepository = sportSupplementRepository;

    public async Task Handle(ApproveCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        await this.sportSupplementRepository.ApproveAsync((int)request.Id);
    }
}