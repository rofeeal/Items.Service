using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Items.Service.Common.DTOs;
using Items.Service.Query.Application.DTOs;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Application.Queries.ItemsTypes;

namespace Items.Service.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ItemTypeLookupController : ControllerBase
    {
        private readonly ILogger<ItemTypeLookupController> _logger;
        private readonly IQueryDispatcher<ItemTypeEntity> _queryDispatcher;

        public ItemTypeLookupController(ILogger<ItemTypeLookupController> logger, IQueryDispatcher<ItemTypeEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllItemTypesAsync()
        {
            try
            {
                var itemTypes = await _queryDispatcher.SendAsync(new FindAllItemsTypesQuery());
                return NormalResponse(itemTypes);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all item types!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("GetDeletedItemTypes")]
        public async Task<ActionResult> GetDeletedItemTypesAsync()
        {
            try
            {
                var itemTypes = await _queryDispatcher.SendAsync(new FindDeletedItemsTypesQuery());
                return NormalResponse(itemTypes);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve deleted items Types!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("byId/{itemTypeId}")]
        public async Task<ActionResult> GetByItemTypeIdAsync(Guid itemTypeId)
        {
            try
            {
                var itemTypes = await _queryDispatcher.SendAsync(new FindItemTypeByIdQuery { Id = itemTypeId });
                return NormalResponse(itemTypes);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find item type by ID!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("byCode/{code}")]
        public async Task<ActionResult> GetItemTypesByCodeAsync(string code)
        {
            try
            {
                var itemTypes = await _queryDispatcher.SendAsync(new FindItemTypeByCodeQuery { Code = code });
                return NormalResponse(itemTypes);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find item types by code!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        private ActionResult NormalResponse(List<ItemTypeEntity> itemTypes)
        {
            if (itemTypes == null || !itemTypes.Any())
                return NoContent();

            var count = itemTypes.Count;
            return Ok(new ItemTypeLookupResponse
            {
                Results = itemTypes,
                Message = $"Successfully returned {count} item type{(count > 1 ? "s" : string.Empty)}!"
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
