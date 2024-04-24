namespace Just4Fit_WorkingStaff.Infrastructure.News.Handlers;

using Just4Fit_WorkingStaff.Infrastructure.News.Queries;
using Just4Fit_WorkingStaff.Core.News.Models;
using MediatR;
using Just4Fit_WorkingStaff.Core.News.Repositories;

public class GetAllHandler : IRequestHandler<GetAllQuery, IEnumerable<News>>
{
    private readonly INewsRepository newsRepository;

    public GetAllHandler(INewsRepository newsRepository) => this.newsRepository = newsRepository;

    public async Task<IEnumerable<News>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var news = await this.newsRepository.GetAllAsync();

        if (news is null)
        {
            return Enumerable.Empty<News>();
        }

        return news;
    }
}