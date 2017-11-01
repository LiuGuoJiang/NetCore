using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Dtos
{
    public class Material
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public Product Product { get; set; }
    }
}
