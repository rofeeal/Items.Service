using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Items.Service.Common.DTOs;
using Items.Service.Query.Application.DTOs;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Application.Queries.ItemsCategories;

namespace Items.Service.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ItemCategoryLookupController : ControllerBase
    {
        private readonly ILogger<ItemCategoryLookupController> _logger;
        private readonly IQueryDispatcher<ItemCategoryEntity> _queryDispatcher;

        public ItemCategoryLookupController(ILogger<ItemCategoryLookupController> logger, IQueryDispatcher<ItemCategoryEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllItemCategoriesAsync()
        {
            try
            {
                var itemCategories = await _queryDispatcher.SendAsync(new FindAllItemsCategoriesQuery());
                return NormalResponse(itemCategories);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all item types!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("GetDeletedItemCategories")]
        public async Task<ActionResult> GetDeletedItemCategoriesAsync()
        {
            try
            {
                var itemCategories = await _queryDispatcher.SendAsync(new FindDeletedItemsCategoriesQuery());
                return NormalResponse(itemCategories);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve deleted items Categories!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("byId/{itemCategoryId}")]
        public async Task<ActionResult> GetByItemCategoryIdAsync(Guid itemCategoryId)
        {
            try
            {
                var itemCategories = await _queryDispatcher.SendAsync(new FindItemCategoryByIdQuery { Id = itemCategoryId });
                return NormalResponse(itemCategories);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find item type by ID!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("byCode/{code}")]
        public async Task<ActionResult> GetItemCategoriesByCodeAsync(string code)
        {
            try
            {
                var itemCategories = await _queryDispatcher.SendAsync(new FindItemCategoryByCodeQuery { Code = code });
                return NormalResponse(itemCategories);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find item types by code!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        private ActionResult NormalResponse(List<ItemCategoryEntity> itemCategories)
        {
            if (itemCategories == null || !itemCategories.Any())
                return NoContent();

            var count = itemCategories.Count;
            return Ok(new ItemCategoryLookupResponse
            {
                Results = itemCategories,
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
