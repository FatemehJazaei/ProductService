using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Application.Products.Queries.GetAllProducts;
using ProductService.Application.Products.Commands.CreateProduct;
using ProductService.Application.Products.Commands.DeleteProduct;
using ProductService.Application.Products.Commands.UpdateProduct;
using ProductService.Application.Products.Queries.GetAllProducts;
using ProductService.Application.Products.Queries;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// همه محصولات را برمی‌گرداند
        /// </summary>
        /// <param name="createdByUserId">فیلتر براساس کاربر سازنده (اختیاری)</param>
        /// <returns>لیست محصولات</returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "دریافت همه محصولات",
            Description = "تمام محصولات ثبت شده را برمی‌گرداند. امکان فیلتر براساس کاربر سازنده وجود دارد."
        )]

        public async Task<IActionResult> GetAll([FromQuery] Guid? createdByUserId)
        {
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }

        /// <summary>
        /// دریافت محصول براساس شناسه
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>جزئیات محصول</returns>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "دریافت محصول با شناسه",
            Description = "محصول موردنظر را براساس شناسه برمی‌گرداند."
        )]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProductsByUserQuery(id));
            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// ایجاد محصول جدید
        /// </summary>
        /// <param name="command">اطلاعات محصول جدید</param>
        /// <returns>شناسه محصول ایجاد شده</returns>
        [HttpPost]
        [Authorize]
        [SwaggerOperation(
            Summary = "ایجاد محصول جدید",
            Description = "یک محصول جدید را ایجاد می‌کند."
        )]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        /// <summary>
        /// ویرایش محصول موجود
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <param name="command">اطلاعات ویرایش شده محصول</param>
        /// <returns>بدون خروجی</returns>
        [HttpPut("{id:guid}")]
        [Authorize(Policy = "OwnerOnly")]
        [SwaggerOperation(
            Summary = "ویرایش محصول",
            Description = "محصول موجود را براساس شناسه ویرایش می‌کند."
        )]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id) return BadRequest("Product ID mismatch");

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// حذف محصول موجود
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>بدون خروجی</returns>
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "OwnerOnly")]
        [SwaggerOperation(
            Summary = "حذف محصول",
            Description = "محصول موجود را براساس شناسه حذف می‌کند."
        )]
        public async Task<IActionResult> Delete(Guid id)
        {
            var CreatedByUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _mediator.Send(new DeleteProductCommand(id, CreatedByUserId));
            return NoContent();
        }
    }
}
