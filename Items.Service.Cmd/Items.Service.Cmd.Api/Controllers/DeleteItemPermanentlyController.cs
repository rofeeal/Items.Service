using CQRS.Core.Exceptions;
using CQRS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Items.Service.Common.DTOs;
using Items.Service.Cmd.Application.Commands.Items;

namespace Items.Service.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeleteItemPermanentlyController : ControllerBase
    {
        private readonly ILogger<DeleteItemPermanentlyController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public DeleteItemPermanentlyController(ILogger<DeleteItemPermanentlyController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemPermanentlyAsync(Guid id, DeleteItemPermanentlyCommand command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return Ok(new BaseResponse
                {
                    Message = "Delete item permanently request completed successfully!"
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
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, item passed an incorrect item ID targetting the aggregate!");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to permanently delete an item!";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}