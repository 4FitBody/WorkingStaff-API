namespace Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Handlers;

using Just4Fit_WorkingStaff.Core.SportSupplements.Models.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Commands;
using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand>
{
    private readonly ISportSupplementRepository supplementRepository;
    public CreateHandler(ISportSupplementRepository supplementRepository) => this.supplementRepository = supplementRepository;

     public async Task Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.SportSupplement);

        await this.supplementRepository.CreateAsync(request.SportSupplement);
    }

}
