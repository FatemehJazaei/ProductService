using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using ProductApp.Application.Products.Queries.GetAllProducts;
using ProductService.Domain.Entities;

namespace ProductService.Application.Common.Mappings
{
    public class ProductMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Product, ProductDto>();
        }
    }
}
