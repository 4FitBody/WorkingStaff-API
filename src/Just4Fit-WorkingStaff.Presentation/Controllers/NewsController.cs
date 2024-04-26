namespace Just4Fit_WorkingStaff.Presentation.Controllers;

using Just4Fit_WorkingStaff.Core.News.Models;
using Just4Fit_WorkingStaff.Infrastructure.News.Commands;
using Just4Fit_WorkingStaff.Infrastructure.News.Queries;
using Just4Fit_WorkingStaff.Infrastructure.Services;
using Just4Fit_WorkingStaff.Presentation.News.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]/[action]")]
public class NewsController : ControllerBase
{
    private readonly ISender sender;
    private readonly BlobContainerService blobContainerService;

    public NewsController(ISender sender)
    {
        this.sender = sender;

        this.blobContainerService = new BlobContainerService();
    }

    [HttpGet]
    [ActionName("Index")]
    public async Task<IActionResult> GetAll()
    {
        var getAllQuery = new GetAllQuery();

        var news = await this.sender.Send(getAllQuery);

        return base.Ok(news);
    }

    [HttpGet]
    [Route("/api/[controller]/[action]/{id}")]
    public async Task<IActionResult> Details(int? id)
    {
        var getByIdQuery = new GetByIdQuery(id);

        var news = await this.sender.Send(getByIdQuery);

        return base.Ok(news);
    }

    [HttpPost]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> Create(object newsContentJson)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        var newsContent = JsonConvert.DeserializeObject<NewsContent>(newsContentJson.ToString()!, settings);  

        var imageFileName = newsContent!.ImageFileName;

        var imageFileData = newsContent.ImageFileContent;

        var rawPath = Guid.NewGuid().ToString() + imageFileName;

        var path = rawPath.Replace(" ", "%20");

        var news = newsContent!.News!;

        news.CreationDate = DateTime.Now;

        news.ImageUrl = "https://4fitbodystorage.blob.core.windows.net/images/" + path;

        await this.blobContainerService.UploadAsync(new MemoryStream(imageFileData!), rawPath);

        news.IsApproved = true;

        var createCommand = new CreateCommand(news);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }

    [HttpDelete]
    [Route("/api/[controller]/[action]/{id}")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> Delete(int? id)
    {
        var createCommand = new DeleteCommand(id);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }


    [HttpPut]
    [Route("/api/[controller]/[action]/{id}")]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> Update(int? id, [FromBody] News news)
    {
        var createCommand = new UpdateCommand(id, news);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }
}