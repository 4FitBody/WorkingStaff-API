namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Handlers;

using Just4Fit_WorkingStaff.Core.SportSupplements.Models;
using Just4Fit_WorkingStaff.Core.SportSupplements.Models.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Queries;
using MediatR;

public class GetAllHandler : IRequestHandler<GetAllQuery, IEnumerable<SportSupplement>>
{
    private readonly ISportSupplementRepository sportSupplementRepository;

    public GetAllHandler(ISportSupplementRepository sportSupplementRepository) => this.sportSupplementRepository = sportSupplementRepository;

    public async Task<IEnumerable<SportSupplement>?> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var supplements = await this.sportSupplementRepository.GetAllAsync();

        if (supplements is null)
        {
            return Enumerable.Empty<SportSupplement>();
        }

        return supplements;
    }
}
