using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class User
    {
        public Guid Id { get;  set; }
        public string UserName { get;  set; } 
        public string PasswordHash { get;  set; }
        public DateTime CreatedAt { get; set; }

        public Collection<Product>? Products { get; set; }
    }
}
