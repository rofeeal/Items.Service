﻿using CQRS.Core.Exceptions;
using CQRS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Items.Service.Common.DTOs;
using Items.Service.Cmd.Application.Commands.ItemsTypes;

namespace Items.Service.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeleteItemTypeController : ControllerBase
    {
        private readonly ILogger<DeleteItemTypeController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public DeleteItemTypeController(ILogger<DeleteItemTypeController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemTypeAsync(Guid id, DeleteItemTypeCommand command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return Ok(new BaseResponse
                {
                    Message = "Delete item type request completed successfully!"
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
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, item type passed an incorrect item type ID targetting the aggregate!");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to delete a item type!";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}