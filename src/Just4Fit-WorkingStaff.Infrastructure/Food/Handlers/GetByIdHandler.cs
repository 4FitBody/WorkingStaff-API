namespace Just4Fit_WorkingStaff.Infrastructure.Food.Handlers;

using System;
using System.Linq;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.Food.Repositories;
using Just4Fit_WorkingStaff.Core.Food.Models;
using Just4Fit_WorkingStaff.Infrastructure.Food.Queries;
using MediatR;

public class GetByIdHandler : IRequestHandler<GetByIdQuery, Food>
{
    private readonly IFoodRepository repository;

    public GetByIdHandler(IFoodRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Food> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var food = await repository.GetByIdAsync(request.Id);

        if (food is null)
        {
            throw new ArgumentNullException("No food by this Id");
        }

        return food;
    }
}