namespace Just4Fit_WorkingStaff.Infrastructure.Food.Handlers;

using Just4Fit_WorkingStaff.Core.Food.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Food.Commands;
using MediatR;

public class ApproveHandler : IRequestHandler<ApproveCommand>
{
    private readonly IFoodRepository foodRepository;

    public ApproveHandler(IFoodRepository foodRepository) => this.foodRepository = foodRepository;

    public async Task Handle(ApproveCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        await this.foodRepository.ApproveAsync((int)request.Id);
    }
}