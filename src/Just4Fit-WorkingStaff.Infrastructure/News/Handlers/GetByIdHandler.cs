namespace Just4Fit_WorkingStaff.Infrastructure.News.Handlers;

using Just4Fit_WorkingStaff.Infrastructure.News.Queries;
using Just4Fit_WorkingStaff.Core.News.Models;
using MediatR;
using Just4Fit_WorkingStaff.Core.News.Repositories;

public class GetByIdHandler : IRequestHandler<GetByIdQuery, News>
{
    private readonly INewsRepository newsRepository;

    public GetByIdHandler(INewsRepository newsRepository) => this.newsRepository = newsRepository;

    public async Task<News> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        var news = await this.newsRepository.GetByIdAsync((int)request.Id);

        if (news is null)
        {
            return new News();
        }

        return news!;
    }
}