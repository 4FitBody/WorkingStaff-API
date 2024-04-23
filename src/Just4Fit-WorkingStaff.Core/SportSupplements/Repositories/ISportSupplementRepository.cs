namespace Just4Fit_WorkingStaff.Core.SportSupplements.Models.Repositories;

using Just4Fit_WorkingStaff.Core.SportSupplements.Models;

public interface ISportSupplementRepository
{
    Task<IEnumerable<SportSupplement>> GetAllAsync();
    Task CreateAsync(SportSupplement supplement);
    Task UpdateAsync(int id, SportSupplement updatedSupplement);
    Task DeleteAsync(int id);
    Task<SportSupplement> GetByIdAsync(int id);
}
