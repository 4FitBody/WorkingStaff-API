namespace Just4Fit_WorkingStaff.Core.News.Repositories;

using Just4Fit_WorkingStaff.Core.News.Models;

public interface INewsRepository
{
    Task<IEnumerable<News>?> GetAllAsync();
    Task CreateAsync(News news);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, News news);
    Task<News> GetByIdAsync(int id);
}