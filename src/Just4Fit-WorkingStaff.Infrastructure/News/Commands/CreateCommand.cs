namespace Just4Fit_WorkingStaff.Infrastructure.News.Commands;

using Just4Fit_WorkingStaff.Core.News.Models;
using MediatR;

public class CreateCommand : IRequest
{
    public News? News { get; set; }

    public CreateCommand(News? news) => this.News = news;

    public CreateCommand() {}
}