using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Common.Interfaces;

namespace ProductService.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                throw new KeyNotFoundException("Product not found");

            if (entity.CreatedByUserId != request.CurrentUserId)
                throw new UnauthorizedAccessException("You cannot delete this product");

            _context.Products.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }



}
