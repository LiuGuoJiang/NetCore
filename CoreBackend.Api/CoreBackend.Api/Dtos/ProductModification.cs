using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Dtos
{
    public class ProductModification
    {
        [Display(Name = "产品名称")]
        [Required(ErrorMessage = "{0}是必填项")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "{0}的长度应该不小于{2}, 不大于{1}")]
        public string Name { get; set; }

        [Display(Name = "价格")]
        [Range(0, Double.MaxValue, ErrorMessage = "{0}的值必须大于{1}")]
        public decimal Price { get; set; }
        [Display(Name="描述")]
        public string Description { get; set; }
    }
}
