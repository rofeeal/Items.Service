using CQRS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Items.Service.Cmd.Application.DTOs;
using Items.Service.Common.DTOs;
using Items.Service.Cmd.Application.Commands.ItemsCategories;

namespace Items.Service.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewItemCategoryController : ControllerBase
    {
        private readonly ILogger<NewItemCategoryController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewItemCategoryController(ILogger<NewItemCategoryController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> NewItemCategoryAsync(NewItemCategoryCommand command)
        {
            var id = Guid.NewGuid();
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewItemCategoryResponse
                {
                    Id = id,
                    Message = "New item category creation request completed successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Item category made a bad request!");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new item category!";
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