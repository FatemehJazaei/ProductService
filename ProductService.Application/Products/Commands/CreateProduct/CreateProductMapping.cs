using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using ProductService.Domain.Entities;

namespace ProductService.Application.Products.Commands.CreateProduct
{
    public class CreateProductMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateProductCommand, Product>()
                .Map(dest => dest.Id, _ => Guid.NewGuid())
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow);
        }
    }
}
