using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Dtos
{
    public class ProductDto
    {
        public ProductDto()
        {
            Materials = new List<MaterialDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public ICollection<MaterialDto> Materials { get; set; }

        public int MaterialCount => Materials.Count;
    }
}
