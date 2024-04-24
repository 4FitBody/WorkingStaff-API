namespace Just4Fit_WorkingStaff.Infrastructure.News.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.News.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.News.Commands;
using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand>
{
    private readonly INewsRepository newsRepository;

    public CreateHandler(INewsRepository newsRepository) => this.newsRepository = newsRepository;

    public async Task Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.News);

        await this.newsRepository.CreateAsync(request.News);
    }
}