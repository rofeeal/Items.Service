using CQRS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Items.Service.Cmd.Application.DTOs;
using Items.Service.Common.DTOs;
using Items.Service.Cmd.Application.Commands.ItemsTypes;

namespace Items.Service.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewItemTypeController : ControllerBase
    {
        private readonly ILogger<NewItemTypeController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewItemTypeController(ILogger<NewItemTypeController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> NewItemTypeAsync(NewItemTypeCommand command)
        {
            var id = Guid.NewGuid();
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewItemTypeResponse
                {
                    Id = id,
                    Message = "New item type creation request completed successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Item type made a bad request!");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new item type!";
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