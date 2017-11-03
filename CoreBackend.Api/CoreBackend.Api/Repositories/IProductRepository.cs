using CoreBackend.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProduct();
        Product GetProduct(int productId, bool includeMaterials=false);
        IEnumerable<Material> GetMaterialsForProduct(int productId);
        Material GetMaterialForProduct(int productId, int materialId);
        bool ProductExists(int productId);
        void AddProduct(Product product);
        bool Save();
        void DeleteProduct(Product product);
    }
}
