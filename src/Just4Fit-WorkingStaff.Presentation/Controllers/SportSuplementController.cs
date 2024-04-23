namespace Just4Fit_WorkingStaff.Presentation.Controllers;

using Just4Fit_WorkingStaff.Core.SportSupplements.Models;
using Just4Fit_WorkingStaff.Infrastructure.Services;
using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Commands;
using Just4Fit_WorkingStaff.Infrastructure.SportSupplements.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        return base.Ok(suplements);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        var getByIdQuery = new GetByIdQuery(id);

        var sportSupplement = await this.sender.Send(getByIdQuery);

        return base.Ok(sportSupplement);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] SportSupplement sportSupplement, IFormFile file)
    {
        var rawPath = Guid.NewGuid().ToString() + file.FileName;

        var path = rawPath.Replace(" ", "%20");

        var supplement = new SportSupplement
        {
            Name = sportSupplement.Name,
            Description = sportSupplement.Description,
            ManufactureCountry = sportSupplement.ManufactureCountry,
            Quantity = sportSupplement.Quantity,
            IsApproved = false,
            ImageUrl = "https://4fitbodystorage.blob.core.windows.net/images/" + path,
        };

        await this.blobContainerService.UploadAsync(file.OpenReadStream(), rawPath);

        var createCommand = new CreateCommand(supplement);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }

    [HttpDelete]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> Delete(int? id)
    {
        var createCommand = new DeleteCommand(id);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }

    [HttpPut]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> Update(int? id, [FromBody] SportSupplement sportSupplement)
    {
        var createCommand = new UpdateCommand(id, sportSupplement);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }
}
