namespace Just4Fit_WorkingStaff.Presentation.Controllers;

using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.Food.Models;
using Just4Fit_WorkingStaff.Infrastructure.Food.Commands;
using Just4Fit_WorkingStaff.Infrastructure.Food.Queries;
using Just4Fit_WorkingStaff.Infrastructure.Services;
using Just4Fit_WorkingStaff.Presentation.Food.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]/[action]")]
public class FoodController : ControllerBase
{
    private readonly ISender sender;
    private readonly BlobContainerService blobContainerService;

    public FoodController(ISender sender)
    {
        this.sender = sender;

        this.blobContainerService = new BlobContainerService();
    }

    [HttpGet]
    [ActionName("Index")]
    public async Task<IActionResult> GetAll()
    {
        var getAllQuery = new GetAllQuery();

        var allFood = await this.sender.Send(getAllQuery);

        return base.Ok(allFood.Where(food => food.IsApproved));
    }

    [HttpPost]
    public async Task<IActionResult> Create(object foodContentJson)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        var foodContent = JsonConvert.DeserializeObject<FoodContent>(foodContentJson.ToString(), settings);  

        var food = foodContent.Food;

        var imageFileName = foodContent.ImageFileName;

        var imageFileData = foodContent.ImageFileContent;

        var contentFileName = foodContent.VideoFileName;

        var contentFileData = foodContent.VideoFileContent;

        var rawPath = Guid.NewGuid().ToString() + imageFileName;

        var path = rawPath.Replace(" ", "%20");

        food.ImageUrl = "https://4fitbodystorage.blob.core.windows.net/images/" + path;

        await this.blobContainerService.UploadAsync(new MemoryStream(imageFileData!), rawPath);

        var videoRawPath = Guid.NewGuid().ToString() + contentFileName;

        var videoPath = videoRawPath.Replace(" ", "%20");

        food.VideoUrl = "https://4fitbodystorage.blob.core.windows.net/images/" + videoPath;

        await this.blobContainerService.UploadAsync(new MemoryStream(contentFileData!), videoRawPath);

        var createCommand = new CreateCommand(food);

        await this.sender.Send(createCommand);

        return base.Ok();
    }

    [HttpDelete]
    [Route("/api/[controller]/[action]/{id}")]
    [Authorize(Roles = "Nutritionist, Moderator")]
    public async Task<IActionResult> Delete(int? id)
    {
        var createCommand = new DeleteCommand(id);

        await this.sender.Send(createCommand);

        return base.Ok();
    }

    [HttpPut]
    [Route("/api/[controller]/[action]/{id}")]
    [Authorize(Roles = "Nutritionist")]
    public async Task<IActionResult> Update(int? id, [FromBody] Food food)
    {
        var createCommand = new UpdateCommand(id, food);

        await this.sender.Send(createCommand);

        return base.Ok();
    }

    [HttpGet]
    [Route("/api/[controller]/[action]/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var getByIdQuery = new GetByIdQuery(id);

        var food = await this.sender.Send(getByIdQuery);

        return base.Ok(food);
    }
}
