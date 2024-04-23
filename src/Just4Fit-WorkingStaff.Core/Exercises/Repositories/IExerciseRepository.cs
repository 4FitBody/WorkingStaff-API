namespace Just4Fit_WorkingStaff.Core.Exercises.Repositories;

using Just4Fit_WorkingStaff.Core.Exercises.Models;

public interface IExerciseRepository
{
    Task<IEnumerable<Exercise>?> GetAllAsync();
    Task CreateAsync(Exercise exercise);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, Exercise exercise);
    Task<Exercise> GetByIdAsync(int id);
}