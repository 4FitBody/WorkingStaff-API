namespace Just4Fit_WorkingStaff.Presentation.Controllers;

using Just4Fit_WorkingStaff.Core.SportSupplements.Models;
using Just4Fit_WorkingStaff.Infrastructure.Services;
using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Commands;
using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Queries;
using Just4Fit_WorkingStaff.Presentation.SportSuplements.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]/[action]")]
public class SportSuplementController : ControllerBase
{
    private readonly ISender sender;
    private readonly BlobContainerService blobContainerService;

    public SportSuplementController(ISender sender)
    {
        this.sender = sender;

        this.blobContainerService = new BlobContainerService();
    }

    [HttpGet]
    [ActionName("Index")]
    public async Task<IActionResult> GetAll()
    {
        var getAllQuery = new GetAllQuery();

        var suplements = await this.sender.Send(getAllQuery);

        return base.Ok(suplements.Where(suplement => suplement.IsApproved));
    }

    [HttpGet]
    [Route("/api/[controller]/[action]/{id}")]
    public async Task<IActionResult> Details(int? id)
    {
        var getByIdQuery = new GetByIdQuery(id);

        var sportSupplement = await this.sender.Send(getByIdQuery);

        return base.Ok(sportSupplement);
    }

    [HttpPost]
    [Authorize(Roles = "Trainer, Nutritionist")]
    public async Task<IActionResult> Create(object sportSupplementContent)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        var supplementContent = JsonConvert.DeserializeObject<SportSupplementContent>(sportSupplementContent.ToString()!, settings);  

        var imageFileName = supplementContent!.ImageFileName;

        var imageFileData = supplementContent.ImageFileContent;

        var rawPath = Guid.NewGuid().ToString() + imageFileName;

        var path = rawPath.Replace(" ", "%20");

        var supplement = new SportSupplement
        {
            Name = supplementContent!.SportSupplement!.Name,
            Description = supplementContent!.SportSupplement!.Description,
            ManufactureCountry = supplementContent!.SportSupplement!.ManufactureCountry,
            Quantity = supplementContent!.SportSupplement!.Quantity,
            IsApproved = false,
            ImageUrl = "https://4fitbodystorage.blob.core.windows.net/images/" + path,
        };

        await this.blobContainerService.UploadAsync(new MemoryStream(imageFileData), rawPath);

        var createCommand = new CreateCommand(supplement);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }

    [HttpDelete]
    [Route("/api/[controller]/[action]/{id}")]
    [Authorize(Roles = "Trainer, Nutritionist, Moderator")]
    public async Task<IActionResult> Delete(int? id)
    {
        var createCommand = new DeleteCommand(id);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }

    [HttpPut]
    [Route("/api/[controller]/[action]/{id}")]
    [Authorize(Roles = "Trainer, Nutritionist")]
    public async Task<IActionResult> Update(int? id, [FromBody] SportSupplement sportSupplement)
    {
        var createCommand = new UpdateCommand(id, sportSupplement);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }
}
