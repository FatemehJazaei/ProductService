using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductApp.Application.Products.Queries;
using ProductService.Application.Common.Interfaces;
using ProductService.Application.Products.Dto;

namespace ProductApp.Application.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery() : IRequest<List<ProductDto>>;

    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products.ToListAsync(cancellationToken);
            return _mapper.Map<List<ProductDto>>(products); // Mapster
        }
    }



}