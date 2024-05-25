using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDishForRestaurant;
using Restaurants.Application.Dishes.Commands.UpdateDishForRestaurant;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/Restaurants/{restaurantId}/Dishes")]
[Authorize]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = PolicyNames.AtLeast20)]
    public async Task<ActionResult<DishDto>> GetAllForRestaurant([FromRoute] Guid restaurantId)
    {
        var restaurant = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
        return Ok(restaurant);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DishDto>> GetById([FromRoute]Guid restaurantId, [FromRoute] Guid id)
    {
        var restaurant = await mediator.Send(new GetDishByIdForRestaurantQuery(id, restaurantId));
        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] Guid restaurantId, CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { restaurantId, id }, null);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDish([FromRoute] Guid restaurantId, [FromRoute] Guid id)
    {
        await mediator.Send(new DeleteDishForRestaurantCommand(id, restaurantId));
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDish([FromRoute] Guid restaurantId, [FromRoute] Guid id, UpdateDishForRestaurantCommand command)
    {
        command.Id = id;
        command.RestaurantId = restaurantId;
        await mediator.Send(command);

        return NoContent();
    }
}
