namespace Just4Fit_WorkingStaff.Infrastructure.News.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.News.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.News.Commands;
using MediatR;

public class DeleteHandler : IRequestHandler<DeleteCommand>
{
    private readonly INewsRepository newsRepository;

    public DeleteHandler(INewsRepository newsRepository) => this.newsRepository = newsRepository;

    public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        await this.newsRepository.DeleteAsync((int)request.Id);
    }
}