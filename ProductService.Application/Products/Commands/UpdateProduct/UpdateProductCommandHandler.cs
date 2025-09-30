using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Common.Interfaces;

namespace ProductService.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                throw new KeyNotFoundException("Product not found");

            if (entity.CreatedByUserId != request.CurrentUserId)
                throw new UnauthorizedAccessException("You cannot update this product");

            var exists = await _context.Products.AnyAsync(p =>
                p.Id != request.Id &&
                p.ManufactureEmail == request.ManufactureEmail &&
                p.ProduceDate == request.ProduceDate, cancellationToken);

            if (exists)
                throw new InvalidOperationException("Duplicate product with same ManufactureEmail and ProduceDate");

            request.Adapt(entity);

            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
