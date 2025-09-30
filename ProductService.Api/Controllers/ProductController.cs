using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Application.Products.Queries.GetAllProducts;
using ProductService.Application.Products.Commands;
using ProductService.Application.Products.Commands.CreateProduct;
using ProductService.Application.Products.Commands.DeleteProduct;
using ProductService.Application.Products.Commands.UpdateProduct;
using ProductService.Application.Products.Queries;
using ProductService.Application.Products.Queries.GetAllProducts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProductApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] Guid? createdByUserId)
        {
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProductsByUserQuery(id));
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "OwnerOnly")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id) return BadRequest("Product ID mismatch");

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "OwnerOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var CreatedByUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _mediator.Send(new DeleteProductCommand(id, CreatedByUserId));
            return NoContent();
        }
    }
}
