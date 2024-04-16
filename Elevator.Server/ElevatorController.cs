using Microsoft.AspNetCore.Mvc;

namespace Elevator.Server;
[Controller]
[Route("[controller]")]
public class ElevatorController(ElevatorHandler handler) : Controller
{
    /// <summary>
    /// Creates Request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A newly created TodoItem</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /Todo
    ///     {
    ///        "Floor": 10,
    ///        "Direction": "Up"
    ///     }
    /// 
    /// </remarks>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">If the item is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<IActionResult> Create([FromBody]ElevatorRequest? request)
    {
        if (request == null) return Task.FromResult<IActionResult>(BadRequest());
        handler.AddRequest(request);
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status201Created));
    }
    /// <summary>
    /// Starts Elevator.
    /// </summary>
    /// <response code="200">Returns the newly created item</response>
    [HttpPost]
    [Route("Start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> Start()
    {
        handler.Start();
        return Task.FromResult<IActionResult>(Ok());
    }
    /// <summary>
    /// Stops Elevator.
    /// </summary>
    /// <response code="200">Returns the newly created item</response>
    [HttpPost]
    [Route("Stop")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> Stop()
    {
        handler.Stop();
        return Task.FromResult<IActionResult>(Ok());
    }
}