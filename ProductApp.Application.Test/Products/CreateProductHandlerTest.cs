using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Common.Interfaces;
using ProductService.Application.Products.Commands.CreateProduct;
using ProductService.Domain.Entities;
using ProductService.Persistence;
using Xunit;

namespace ProductApp.Application.Test.Products
{
    public class CreateProductHandlerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public CreateProductHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _userId = Guid.NewGuid(); 
            _currentUserService = new FakeCurrentUserService(_userId);

            _mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        }

        [Fact]
        public async Task Handle_Should_Create_Product_Successfully()
        {
            var handler = new CreateProductHandler(_context, _mapper, _currentUserService);

            var command = new CreateProductCommand
            {
                Name = "Laptop",
                IsAvailable = true,
                ManufactureEmail = "test@mail.com",
                ManufacturePhone = "+1234567890",
                ProduceDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            var result = await handler.Handle(command, CancellationToken.None);

            var product = await _context.Products.FindAsync(result);

            Assert.NotNull(product);
            Assert.Equal("Laptop", product!.Name);
            Assert.Equal(_userId, product.CreatedByUserId);
        }
    }

    // Fake Service
    public class FakeCurrentUserService : ICurrentUserService
    {
        public FakeCurrentUserService(Guid userId)
        {
            UserId = userId;
        }

        public Guid? UserId { get; }
    }
}
