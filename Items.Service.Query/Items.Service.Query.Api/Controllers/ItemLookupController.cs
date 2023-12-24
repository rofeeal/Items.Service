using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Items.Service.Common.DTOs;
using Items.Service.Query.Application.DTOs;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Application.Queries.Items;

namespace Items.Service.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ItemLookupController : ControllerBase
    {
        private readonly ILogger<ItemLookupController> _logger;
        private readonly IQueryDispatcher<ItemEntity> _queryDispatcher;

        public ItemLookupController(ILogger<ItemLookupController> logger, IQueryDispatcher<ItemEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllItemsAsync()
        {
            try
            {
                var items = await _queryDispatcher.SendAsync(new FindAllItemsQuery());
                return NormalResponse(items);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all items!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("GetDeletedItems")]
        public async Task<ActionResult> GetDeletedItemsAsync()
        {
            try
            {
                var items = await _queryDispatcher.SendAsync(new FindDeletedItemsQuery());
                return NormalResponse(items);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve deleted items!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("byId/{itemId}")]
        public async Task<ActionResult> GetByItemIdAsync(Guid itemId)
        {
            try
            {
                var items = await _queryDispatcher.SendAsync(new FindItemByIdQuery { Id = itemId });
                return NormalResponse(items);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find item by ID!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("byCode/{code}")]
        public async Task<ActionResult> GetItemsByCodeAsync(string code)
        {
            try
            {
                var items = await _queryDispatcher.SendAsync(new FindItemByCodeQuery { Code = code });
                return NormalResponse(items);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find items by code!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        private ActionResult NormalResponse(List<ItemEntity> items)
        {
            if (items == null || !items.Any())
                return NoContent();

            var count = items.Count;
            return Ok(new ItemLookupResponse
            {
                Results = items,
                Message = $"Successfully returned {count} item{(count > 1 ? "s" : string.Empty)}!"
            });
        }

        private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
        {
            _logger.LogError(ex, safeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = safeErrorMessage
            });
        }
    }
}
