namespace Just4Fit_WorkingStaff.Infrastructure.News.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.News.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.News.Commands;
using MediatR;

public class UpdateHandler : IRequestHandler<UpdateCommand>
{
    private readonly INewsRepository newsRepository;

    public UpdateHandler(INewsRepository newsRepository) => this.newsRepository = newsRepository;

    public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        ArgumentNullException.ThrowIfNull(request.News);

        await this.newsRepository.UpdateAsync((int)request.Id, request.News);
    }
}