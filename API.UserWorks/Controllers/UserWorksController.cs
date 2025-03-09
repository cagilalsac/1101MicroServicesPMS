#nullable disable
using APP.UserWorks.Features.UserWorks;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//Generated from Custom Template.
namespace API.UserWorks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserWorksController : ControllerBase
    {
        private readonly ILogger<UserWorksController> _logger;
        private readonly IMediator _mediator;

        public UserWorksController(ILogger<UserWorksController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Gateway()
        {
            return Ok("UserWorks Gateway");
        }

        // GET: api/UserWorks
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _mediator.Send(new UserWorkQueryRequest());
                if (list.Any())
                    return Ok(list);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("UserWorksGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during UserWorksGet.")); 
            }
        }

        // GET: api/UserWorks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                    return NoContent();
                var list = await _mediator.Send(new UserWorkQueryRequest() { Id = id });
                if (list.Any())
                    return Ok(list.Single());
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("UserWorksGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during UserWorksGetById."));
            }
        }

        // POST: api/UserWorks
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(UserWorkCreateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        //return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
                        return Ok(response);
                    }
                    ModelState.AddModelError("UserWorksPost", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("UserWorksPost Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during UserWorksPost."));
            }
        }

        // PUT: api/UserWorks
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(UserWorkUpdateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        //return NoContent();
                        return Ok(response);
                    }
                    ModelState.AddModelError("UserWorksPut", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("UserWorksPut Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during UserWorksPut."));
            }
        }

        // DELETE: api/UserWorks/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new UserWorkDeleteRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    //return NoContent();
                    return Ok(response);
                }
                ModelState.AddModelError("UserWorksDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("UserWorksDelete Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during UserWorksDelete."));
            }
        }
    }
}
