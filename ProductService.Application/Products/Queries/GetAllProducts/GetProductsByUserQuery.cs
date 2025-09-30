using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Common.Interfaces;
using ProductService.Application.Products.Dto;

namespace ProductService.Application.Products.Queries.GetAllProducts
{
    public record GetProductsByUserQuery(Guid UserId) : IRequest<List<ProductDto>>;

    public class GetProductsByUserHandler : IRequestHandler<GetProductsByUserQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductsByUserHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetProductsByUserQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products
                .Where(p => p.CreatedByUserId == request.UserId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<ProductDto>>(products); // Mapster
        }
    }
}
