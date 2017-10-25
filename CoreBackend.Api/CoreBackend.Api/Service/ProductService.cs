using CoreBackend.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Service
{
    public class ProductService
    {
        public static ProductService CurrentProducts { get; } = new ProductService();
        public List<Product> Products { get; }
        private ProductService()
        {
            Products = new List<Product>
            {
                new Product
                {
                    Id=1,
                    Name="牛奶",
                    Price=2.5M,
                    Description="这是牛奶",
                    Materials=new List<Material>
                    {
                        new Material
                        {
                            Id=1,
                            Name="矿泉水"
                        },
                        new Material
                        {
                            Id=2,
                            Name="奶粉"
                        }
                    }
                },
                new Product
                {
                    Id=2,
                    Name="面包",
                    Price=4.5M,
                    Description="这是面包",
                    Materials=new List<Material>
                    {
                        new Material
                        {
                            Id=3,
                            Name="面粉"
                        },
                        new Material
                        {
                            Id=4,
                            Name="鸡蛋"
                        }
                    }
                }
            };
        }
    }
}
