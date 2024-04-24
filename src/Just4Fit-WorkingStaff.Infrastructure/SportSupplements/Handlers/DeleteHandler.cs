namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Handlers;

using System;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.SportSupplements.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Commands;
using MediatR;

public class DeleteHandler : IRequestHandler<DeleteCommand>
{
    private readonly ISportSupplementRepository supplementRepository;

    public DeleteHandler(ISportSupplementRepository supplementRepository) => this.supplementRepository = supplementRepository;

    public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        await this.supplementRepository.DeleteAsync((int)request.Id);
    }
}
