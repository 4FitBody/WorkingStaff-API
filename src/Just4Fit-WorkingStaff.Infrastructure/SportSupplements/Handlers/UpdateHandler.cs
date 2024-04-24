namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Handlers;

using System;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.SportSupplements.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Commands;
using MediatR;

public class UpdateHandler : IRequestHandler<UpdateCommand>
{
    private readonly ISportSupplementRepository supplementRepository;

    public UpdateHandler(ISportSupplementRepository supplementRepository) => this.supplementRepository = supplementRepository;

    public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        ArgumentNullException.ThrowIfNull(request.SportSupplement);

        await this.supplementRepository.UpdateAsync((int)request.Id, request.SportSupplement);
    }
}
