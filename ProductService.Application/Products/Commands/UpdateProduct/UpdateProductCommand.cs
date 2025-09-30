using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ProductService.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public bool IsAvailable { get; set; }
        public string ManufactureEmail { get; set; } = default!;
        public string ManufacturePhone { get; set; } = default!;
        public DateOnly ProduceDate { get; set; }

        public Guid CurrentUserId { get; set; }
    }
}
