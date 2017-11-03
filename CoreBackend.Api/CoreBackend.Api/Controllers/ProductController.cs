using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreBackend.Api.Dtos;
using CoreBackend.Api.Service;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using CoreBackend.Api.Repositories;
using AutoMapper;

namespace CoreBackend.Api.Controllers
{
    [Route("api/product")]//使用[Route("api/[controller]")], 它使得整个Controller下面所有action的uri前缀变成了"/api/product",
                               //其中[controller]表示XxxController.cs中的Xxx(其实是小写).
                               //也可以具体指定, [Route("api/product")], 这样做的好处是, 如果ProductController重构以后改名了, 只要不改Route里面的内容, 那么请求的地址不会发生变化.
    public class ProductController:Controller
    {
        private readonly ILogger<ProductController> _logger;//interface不是具体的实现类
        private readonly IMailService _mailService;
        private readonly IProductRepository _productRepository;
        public ProductController(ILogger<ProductController> logger,IMailService mailService, IProductRepository productRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _productRepository = productRepository;
        }
        [HttpGet]//然后在GetProducts方法上面, 写上HttpGet, 也可以写HttpGet(). 
                 //它里面还可以加参数,例如: HttpGet("all"), 那么这个Action的请求的地址就变成了 "/api/product/All".
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProduct();
            var results = Mapper.Map<IEnumerable<ProductWithoutMaterialDto>>(products);
            //var results = new List<ProductWithoutMaterialDto>();
            //foreach(var product in products)
            //{
            //    results.Add(new ProductWithoutMaterialDto
            //    {
            //        Id=product.Id,
            //        Name=product.Name,
            //        Price=product.Price,
            //        Description=product.Description
            //    });
            //}
            return Ok(results);
        }
        [Route("{id}",Name ="GetProduct")]
        public IActionResult GetProducts(int id,bool includeMaterial=false)
        {
            Product products = new Product();
            try
            {
                products = _productRepository.GetProduct(id,includeMaterial);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"查找Id为{id}的产品时出现了错误！", ex);
                return StatusCode(500, "处理请求的时候发生了错误！");
            }
            if (includeMaterial)
            {
                var productWithMaterialResult = Mapper.Map<ProductDto>(products);
                return Ok(productWithMaterialResult);
            }
            var onlyProductResult = Mapper.Map<ProductWithoutMaterialDto>(products);
            return Ok(onlyProductResult);
        }
        [HttpPost("CreateProduct")]
        public IActionResult Post([FromBody] ProductCreation product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            if (product.Name == "产品")
            {
                ModelState.AddModelError("Name", "产品名称不可以是‘产品’二字");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newProduct = Mapper.Map<Product>(product);
            _productRepository.AddProduct(newProduct);
            if (!_productRepository.Save())
            {
                return StatusCode(500, "保存产品的时候出错");
            }
            var dto = Mapper.Map<ProductWithoutMaterialDto>(newProduct);
            //var maxId = ProductService.CurrentProducts.Products.Max(x => x.Id);
            //var newProduct = new Product
            //{
            //    Id = ++maxId,
            //    Name = product.Name,
            //    Price = product.Price
            //};
            //ProductService.CurrentProducts.Products.Add(newProduct);
            return CreatedAtRoute("GetProduct", new { id = dto.Id }, dto);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] ProductModification productModification)
        {
            if (productModification == null)
            {
                return BadRequest();
            }

            if (productModification.Name == "产品")
            {
                ModelState.AddModelError("Name", "产品的名称不可以是'产品'二字");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = _productRepository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            Mapper.Map(productModification, product);
            if (!_productRepository.Save())
            {
                return StatusCode(500, "保存产品的时候出错");
            }
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult Patch(int id,[FromBody] JsonPatchDocument<ProductModification> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            var productEntity = _productRepository.GetProduct(id);
            if (productEntity == null)
            {
                return NotFound();
            }
            var toPatch = Mapper.Map<ProductModification>(productEntity);
            patchDoc.ApplyTo(toPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (toPatch.Name == "产品")
            {
                ModelState.AddModelError("Name", "产品的名称不可以是'产品'二字");
            }
            TryValidateModel(toPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Mapper.Map(toPatch, productEntity);
            if (!_productRepository.Save())
            {
                return StatusCode(500, "更新的时候出错");
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var model = _productRepository.GetProduct(id);
            if (model == null)
            {
                return NotFound();
            }
            _productRepository.DeleteProduct(model);
            if (!_productRepository.Save())
            {
                return StatusCode(500, "删除数据出错");
            }
            _mailService.Send("Product Deleted", $"Id为{id}的产品被删除");
            return NoContent();
        }
    }
}
