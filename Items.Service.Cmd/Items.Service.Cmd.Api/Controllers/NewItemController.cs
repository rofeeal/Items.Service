using CQRS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Items.Service.Cmd.Application.DTOs;
using Items.Service.Common.DTOs;
using Items.Service.Cmd.Application.Commands.Items;

namespace Items.Service.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewItemController : ControllerBase
    {
        private readonly ILogger<NewItemController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewItemController(ILogger<NewItemController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> NewItemAsync(NewItemCommand command)
        {
            var id = Guid.NewGuid();
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewItemResponse
                {
                    Id = id,
                    Message = "New item creation request completed successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Item made a bad request!");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
			}
			catch (ArgumentException ex)
			{
				_logger.LogWarning(ex, "Error occurred while processing the request: {ErrorMessage}", ex.Message);

				return StatusCode(StatusCodes.Status400BadRequest, new NewItemResponse
				{
					Id = id,
					Message = ex.Message
				});
			}

			catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new item!";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewItemResponse
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}