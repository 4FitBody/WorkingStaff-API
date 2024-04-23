namespace Just4Fit_WorkingStaff.Infrastructure.Food.Handlers;

using Just4Fit_WorkingStaff.Infrastructure.Food.Queries;
using Just4Fit_WorkingStaff.Core.Food.Models;
using MediatR;
using Just4Fit_WorkingStaff.Core.Food.Repositories;

public class GetAllHandler : IRequestHandler<GetAllQuery, IEnumerable<Food>>
{
    private readonly IFoodRepository foodRepository;

    public GetAllHandler(IFoodRepository foodRepository) => this.foodRepository = foodRepository;

    public async Task<IEnumerable<Food>?> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var food = await this.foodRepository.GetAllAsync();

        if (food is null)
        {
            return Enumerable.Empty<Food>();
        }

        return food;
    }
}