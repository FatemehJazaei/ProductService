using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Common.Interfaces;

namespace ProductService.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }

        public DeleteProductCommand(Guid id, Guid currentUserId)
        {
            Id = id;
            CurrentUserId = currentUserId;
        }
    }
}
