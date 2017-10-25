using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreBackend.Api.Dtos;
using CoreBackend.Api.Service;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace CoreBackend.Api.Controllers
{
    [Route("api/product")]//使用[Route("api/[controller]")], 它使得整个Controller下面所有action的uri前缀变成了"/api/product",
                               //其中[controller]表示XxxController.cs中的Xxx(其实是小写).
                               //也可以具体指定, [Route("api/product")], 这样做的好处是, 如果ProductController重构以后改名了, 只要不改Route里面的内容, 那么请求的地址不会发生变化.
    public class ProductController:Controller
    {
        private ILogger<ProductController> _logger;//interface不是具体的实现类
        ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }
        [HttpGet]//然后在GetProducts方法上面, 写上HttpGet, 也可以写HttpGet(). 
                 //它里面还可以加参数,例如: HttpGet("all"), 那么这个Action的请求的地址就变成了 "/api/product/All".
        public IActionResult GetProducts()
        {
            return Ok(ProductService.CurrentProducts.Products);
        }
        [Route("{id}",Name ="GetProduct")]
        public IActionResult GetProducts(int id)
        {
            try
            {
                var products = ProductService.CurrentProducts.Products.SingleOrDefault(x => x.Id == id);
                if (products == null)
                {
                    _logger.LogInformation($"Id为{id}的产品没有被找到");
                    return NotFound();
                }
                else
                {
                    return Ok(products);
                }
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"查找Id为{id}的产品时出现了错误！", ex);
                return StatusCode(500, "处理请求的时候发生了错误！");
            }
        }
        [HttpPost("CreateProduct")]
        public IActionResult Post([FromBody] ProductCreation product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var maxId = ProductService.CurrentProducts.Products.Max(x => x.Id);
            var newProduct = new Product
            {
                Id = ++maxId,
                Name = product.Name,
                Price = product.Price
            };
            ProductService.CurrentProducts.Products.Add(newProduct);
            return CreatedAtRoute("GetProduct", new { id = newProduct.Id },newProduct);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] ProductModification product)
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
                return BadRequest();
            }
            var model = ProductService.CurrentProducts.Products.SingleOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            model.Name = product.Name;
            model.Price = product.Price;
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult Patch(int id,[FromBody] JsonPatchDocument<ProductModification> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            var model = ProductService.CurrentProducts.Products.SingleOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            var toPatch = new ProductModification
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price
            };
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
            model.Name = toPatch.Name;
            model.Description = toPatch.Description;
            model.Price = toPatch.Price;
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var model = ProductService.CurrentProducts.Products.SingleOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            ProductService.CurrentProducts.Products.Remove(model);
            return NoContent();
        }
    }
}
