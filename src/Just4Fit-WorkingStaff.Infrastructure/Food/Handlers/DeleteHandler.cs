namespace Just4Fit_WorkingStaff.Infrastructure.Food.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.Food.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Food.Commands;
using MediatR;

public class DeleteHandler : IRequestHandler<DeleteCommand>
{
    private readonly IFoodRepository foodRepository;

    public DeleteHandler(IFoodRepository foodRepository) => this.foodRepository = foodRepository;

    public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        await this.foodRepository.DeleteAsync((int)request.Id);
    }
}