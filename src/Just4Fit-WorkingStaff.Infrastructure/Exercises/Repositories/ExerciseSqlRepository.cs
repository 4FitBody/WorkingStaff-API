namespace Just4Fit_WorkingStaff.Infrastructure.Exercises.Repositories;

using Just4Fit_WorkingStaff.Core.Exercises.Models;
using Just4Fit_WorkingStaff.Core.Exercises.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ExerciseSqlRepository : IExerciseRepository
{
    private readonly JustForFitWorkingStaffDbContext dbContext;

    public ExerciseSqlRepository(JustForFitWorkingStaffDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Exercise>?> GetAllAsync()
    {
        var exercises = this.dbContext.Exercises.AsEnumerable();

        return exercises;
    }

    public async Task CreateAsync(Exercise exercise)
    {
        exercise.Instructions = exercise.Instructions!.Take(exercise.Instructions!.Length - 1).ToArray();

        exercise.SecondaryMuscles = exercise.SecondaryMuscles!.Take(exercise.SecondaryMuscles!.Length - 1).ToArray();

        await this.dbContext.Exercises.AddAsync(exercise);

        await this.dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var exerciseToDelete = await this.dbContext.Exercises.FirstOrDefaultAsync(exercise => exercise.Id == id);

        this.dbContext.Remove<Exercise>(exerciseToDelete!);

        await this.dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Exercise exercise)
    {
        var oldExercise = await this.dbContext.Exercises.FirstOrDefaultAsync(e => e.Id == id);

#pragma warning disable CS8602
        oldExercise.Name = exercise.Name;
#pragma warning restore CS8602
        oldExercise.Equipment = exercise.Equipment;
        oldExercise.Target = exercise.Target;
        oldExercise.BodyPart = exercise.BodyPart;
        oldExercise.IsApproved = exercise.IsApproved;

        if (exercise.SecondaryMuscles is not null)
        {
            oldExercise.SecondaryMuscles = exercise.SecondaryMuscles;
        }

        if (exercise.Instructions is not null)
        {
            oldExercise.Instructions = exercise.Instructions;
        }

        await this.dbContext.SaveChangesAsync();
    }

    public async Task<Exercise> GetByIdAsync(int id)
    {
        var searchedExercise = await this.dbContext.Exercises.FirstOrDefaultAsync(exercise => exercise.Id == id);
    
        return searchedExercise!;
    }

    public async Task ApproveAsync(int id)
    {
        var searchedExercise = await this.dbContext.Exercises.FirstOrDefaultAsync(exercise => exercise.Id == id);
    
        searchedExercise!.IsApproved = true;

        await this.dbContext.SaveChangesAsync();
    }
}