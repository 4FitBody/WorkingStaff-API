namespace Just4Fit_WorkingStaff.Infrastructure.Food.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.Food.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Food.Commands;
using MediatR;

public class UpdateHandler : IRequestHandler<UpdateCommand>
{
    private readonly IFoodRepository foodRepository;

    public UpdateHandler(IFoodRepository foodRepository) => this.foodRepository = foodRepository;

    public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        ArgumentNullException.ThrowIfNull(request.Food);

        await this.foodRepository.UpdateAsync((int)request.Id, request.Food);
    }
}