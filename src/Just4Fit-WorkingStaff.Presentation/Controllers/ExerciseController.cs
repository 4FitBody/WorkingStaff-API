namespace Just4Fit_WorkingStaff.Presentation.Controllers;

using Just4Fit_WorkingStaff.Core.Exercises.Models;
using Just4Fit_WorkingStaff.Infrastructure.Exercises.Commands;
using Just4Fit_WorkingStaff.Infrastructure.Exercises.Queries;
using Just4Fit_WorkingStaff.Infrastructure.Services;
using Just4Fit_WorkingStaff.Presentation.Exercises.Dtos;
using Just4Fit_WorkingStaff.Presentation.Exercises.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]/[action]")]
public class ExerciseController : ControllerBase
{
    private readonly ISender sender;
    private readonly BlobContainerService blobContainerService;

    public ExerciseController(ISender sender)
    {
        this.sender = sender;

        this.blobContainerService = new BlobContainerService();
    }

    [HttpGet]
    [ActionName("Index")]
    public async Task<IActionResult> GetAll()
    {
        var getAllQuery = new GetAllQuery();

        var exercises = await this.sender.Send(getAllQuery);

        return base.Ok(exercises.Where(exercise => exercise.IsApproved));
    }

    [HttpGet]
    [Route("/api/[controller]/[action]/{id}")]
    public async Task<IActionResult> Details(int? id)
    {
        var getByIdQuery = new GetByIdQuery(id);

        var exercise = await this.sender.Send(getByIdQuery);

        return base.Ok(exercise);
    }

    [HttpPost]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> Create(object exerciseContentJson)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        var exerciseContent = JsonConvert.DeserializeObject<ExerciseContent>(exerciseContentJson.ToString()!, settings);  

        var imageFileName = exerciseContent!.ImageFileName;

        var imageFileData = exerciseContent.ImageFileContent;

        var rawPath = Guid.NewGuid().ToString() + imageFileName;

        var path = rawPath.Replace(" ", "%20");

        var exercise = new Exercise
        {
            Name = exerciseContent.Exercise!.Name,
            BodyPart = exerciseContent.Exercise.BodyPart,
            Equipment = exerciseContent.Exercise.Equipment,
            Target = exerciseContent.Exercise.Target,
            IsApproved = false,
            GifUrl = "https://4fitbodystorage.blob.core.windows.net/images/" + path,
            SecondaryMuscles = exerciseContent.Exercise.SecondaryMuscles!,
            Instructions = exerciseContent.Exercise.Instructions!,
        };

        await this.blobContainerService.UploadAsync(new MemoryStream(imageFileData!), rawPath);

        var createCommand = new CreateCommand(exercise);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }

    [HttpDelete]
    [Route("/api/[controller]/[action]/{id}")]
    [Authorize(Roles = "Trainer, Moderator")]
    public async Task<IActionResult> Delete(int? id)
    {
        var createCommand = new DeleteCommand(id);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }

    [HttpPut]
    [Route("/api/[controller]/[action]/{id}")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> Update(int? id, [FromBody] ExerciseDto exerciseDto)
    {
        var exercise = new Exercise
        {
            Name = exerciseDto.Name,
            BodyPart = exerciseDto.BodyPart,
            Equipment = exerciseDto.Equipment,
            Target = exerciseDto.Target,
            IsApproved = false,
            SecondaryMuscles = exerciseDto.SecondaryMuscles!.Split("; "),
            Instructions = exerciseDto.Instructions!.Split("; "),
        };

        var createCommand = new UpdateCommand(id, exercise);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }
}