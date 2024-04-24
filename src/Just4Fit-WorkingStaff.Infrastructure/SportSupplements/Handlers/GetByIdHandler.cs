namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Handlers;

using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Queries;
using Just4Fit_WorkingStaff.Core.SportSupplements.Models;
using MediatR;
using Just4Fit_WorkingStaff.Core.SportSupplements.Repositories;

public class GetByIdHandler : IRequestHandler<GetByIdQuery, SportSupplement>
{
    private readonly ISportSupplementRepository supplementRepository;

    public GetByIdHandler(ISportSupplementRepository supplementRepository) => this.supplementRepository = supplementRepository;

    public async Task<SportSupplement> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        var sportSupplement = await this.supplementRepository.GetByIdAsync((int)request.Id);

        if (sportSupplement is null)
        {
            return new SportSupplement();
        }

        return sportSupplement!;
    }
}