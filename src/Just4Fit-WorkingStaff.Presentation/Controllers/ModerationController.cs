namespace Just4Fit_WorkingStaff.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize(Roles = "Moderator")]
[Route("api/[controller]/[action]")]
public class ModerationController : ControllerBase
{
    private readonly ISender sender;

    public ModerationController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExercises()
    {
        var getAllQuery = new Infrastructure.Exercises.Queries.GetAllQuery();

        var exercises = await this.sender.Send(getAllQuery);

        return base.Ok(exercises.Where(exercise => !exercise.IsApproved));
    }

    [HttpPut]
    [Route("/api/[controller]/[action]/{id}")]
    public async Task<IActionResult> ApproveExercise(int? id)
    {
        var getByIdQuery = new Infrastructure.Exercises.Commands.ApproveCommand(id);

        await this.sender.Send(getByIdQuery);

        return base.Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFood()
    {
        var getAllQuery = new Infrastructure.Food.Queries.GetAllQuery();

        var allFood = await this.sender.Send(getAllQuery);

        return base.Ok(allFood.Where(food => !food.IsApproved));
    }

    [HttpPut]
    [Route("/api/[controller]/[action]/{id}")]
    public async Task<IActionResult> ApproveFood(int? id)
    {
        var getByIdQuery = new Infrastructure.Food.Commands.ApproveCommand(id);

        await this.sender.Send(getByIdQuery);

        return base.Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSportSupplements()
    {
        var getAllQuery = new Infrastructure.SportSupplements.Queries.GetAllQuery();

        var supplements = await this.sender.Send(getAllQuery);

        return base.Ok(supplements.Where(supplement => !supplement.IsApproved));
    }

    [HttpPut]
    [Route("/api/[controller]/[action]/{id}")]
    public async Task<IActionResult> ApproveSportSupplement(int? id)
    {
        var getByIdQuery = new Infrastructure.SportSupplements.Commands.ApproveCommand(id);

        await this.sender.Send(getByIdQuery);

        return base.Ok();
    }
}
