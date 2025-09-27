using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }         
        public string Name { get;  set; }
        public bool IsAvailable { get;  set; }
        public string ManufactureEmail { get;  set; }
        public string ManufacturePhone { get;  set; }
        public DateOnly ProduceDate { get;  set; }   


        public User CreatedByUser { get;  set; }
        public long CreatedByUserId { get; set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime? UpdatedAt { get;  set; }
            

    }
}
