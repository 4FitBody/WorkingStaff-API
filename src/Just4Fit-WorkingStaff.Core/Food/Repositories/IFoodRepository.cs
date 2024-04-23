namespace Just4Fit_WorkingStaff.Core.Food.Repositories;

using Just4Fit_WorkingStaff.Core.Food.Models;

public interface IFoodRepository
{
    Task<IEnumerable<Food>?> GetAllAsync();
    Task CreateAsync(Food food);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, Food food);
    Task<Food> GetByIdAsync(int id);
}
