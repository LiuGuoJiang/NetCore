using CoreBackend.Api.Dtos;
using CoreBackend.Api.Repositories;
using CoreBackend.Api.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Controllers
{
    [Route("api/product")]
    public class MaterialController:Controller
    {
        private readonly IProductRepository _productRepository;
        public MaterialController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet("{productId}/materials")]
        public IActionResult GetMaterials(int productId)
        {
            var product = _productRepository.ProductExists(productId);
            if (!product)
            {
                return NotFound();
            }
            var materials = _productRepository.GetMaterialsForProduct(productId);
            var results = AutoMapper.Mapper.Map<IEnumerable<MaterialDto>>(materials);
            //var results = materials.Select(material => new Material
            //{
            //    Id=material.Id,
            //    Name=material.Name
            //}).ToList();
            return Ok(results);
        }
        [HttpGet("{productId}/materials/{id}")]
        public IActionResult GetMaterial(int productId, int id)
        {
            var product = _productRepository.ProductExists(productId);
            if (!product)
            {
                return NotFound();
            }
            var material = _productRepository.GetMaterialForProduct(productId,id);
            if (material == null)
            {
                return NotFound();
            }
            var results = AutoMapper.Mapper.Map<MaterialDto>(material);
            //var result = new Material
            //{
            //    Id = material.Id,
            //    Name = material.Name
            //};
            return Ok(results);
        }
    }
}
