using CQRS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Items.Service.Cmd.Application.Commands.ItemsTypes;
using Items.Service.Cmd.Domain.Aggregates;
using Items.Service.Common.DTOs;

namespace Items.Service.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RestoreReadDbItemTypeController : ControllerBase
    {
        private readonly ILogger<RestoreReadDbItemTypeController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public RestoreReadDbItemTypeController(ILogger<RestoreReadDbItemTypeController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> RestoreReadDbAsync()
        {
            try
            {
                await _commandDispatcher.SendAsync(new RestoreReadDbItemTypeCommand() { AggregateType = nameof(ItemTypeAggregate) });

                return StatusCode(StatusCodes.Status201Created, new BaseResponse
                {
                    Message = "Read database restore request completed successfully!"
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to restore read database!";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}