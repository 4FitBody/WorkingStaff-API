namespace Just4Fit_WorkingStaff.Infrastructure.News.Commands;

using Just4Fit_WorkingStaff.Core.News.Models;
using MediatR;

public class UpdateCommand : IRequest
{
    public int? Id { get; set; }
    public News? News { get; set; }

    public UpdateCommand(int? id, News? news)
    {
        this.Id = id;

        this.News = news;
    }

    public UpdateCommand() {}
}