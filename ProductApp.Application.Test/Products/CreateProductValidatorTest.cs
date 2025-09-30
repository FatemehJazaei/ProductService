using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using ProductService.Application.Products.Commands.CreateProduct;

namespace ProductApp.Application.Test.Products
{

    public class CreateProductValidatorTest
    {
        private readonly CreateProductValidator _validator;

        public CreateProductValidatorTest()
        {
            _validator = new CreateProductValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new CreateProductCommand
            {
                Name = "",
                ManufactureEmail = "test@mail.com",
                ManufacturePhone = "+1234567890",
                ProduceDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Request_Is_Valid()
        {
            var command = new CreateProductCommand
            {
                Name = "Laptop",
                ManufactureEmail = "test@mail.com",
                ManufacturePhone = "+1234567890",
                ProduceDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
