using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ECommerce.Controllers
{
    [Route("/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static List<product> Products = new List<product>();
        private static int nextID = 1;

        [HttpGet]

        public IEnumerable<product> Get([FromQuery] ProductFilter pfilter)
        {

            var FinalProducts = Products;
            List<product> filtered_products = new List<product>();
            if (pfilter.category != "")
            {
                var CategoryQuery =
                    from product in Products
                    where (product.Category==pfilter.category)
                    select product;
                FinalProducts= CategoryQuery.ToList();
            }

            if (pfilter.low!=0 || pfilter.high!=decimal.MaxValue)
            {
                var RangeQuery =
                    from product in FinalProducts
                    where (product.Price >= pfilter.low && product.Price <= pfilter.high)
                    select product;
                FinalProducts = RangeQuery.ToList();
            }

            if (pfilter.Availability == true)
            {
                var AvailabilityQuery =
                    from product in FinalProducts
                    where (product.Quantity>0)
                    select product;
                FinalProducts = AvailabilityQuery.ToList();
            }

            return FinalProducts;
            
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = Products.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return BadRequest("Product Not Found");
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Post(product Product)
        {
            var acc_type = HttpContext.Session.GetString("AccountType");
            if(acc_type != "admin")
            {
                return Unauthorized();
            }

            // Manual validation of product object
            if (Product == null || !ValidateProduct(Product))
            {
                return BadRequest("Invalid product input");
            }
            try
            {
                Product.Id = nextID++;
                Products.Add(Product);
                return CreatedAtAction(nameof(Get), new { id = Product.Id }, Product);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while processing your request");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, product updatedProduct)
        {
            var acc_type = HttpContext.Session.GetString("AccountType");
            if (acc_type != "admin")
            {
                return Unauthorized();
            }
            try
            {
                var productToUpdate = Products.FirstOrDefault(p => p.Id == id);
                if (productToUpdate == null)
                {
                    return NotFound("Product not found");
                }
                productToUpdate.Name = updatedProduct.Name;
                productToUpdate.Category = updatedProduct.Category;
                productToUpdate.Price = updatedProduct.Price;
                productToUpdate.Quantity = updatedProduct.Quantity;
                productToUpdate.Description = updatedProduct.Description;
                productToUpdate.ImageURL = updatedProduct.ImageURL;
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Policy = "AdminOnly")]
        public ActionResult Delete(int id)
        {
            var acc_type = HttpContext.Session.GetString("AccountType");
            if (acc_type != "admin")
            {
                return Unauthorized();
            }
            try
            {
                var productToDelete = Products.FirstOrDefault(p => p.Id == id);
                if (productToDelete == null)
                {
                    return NotFound("Product not found");
                }
                Products.Remove(productToDelete);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool ValidateProduct(product Product)
        {
            if (Product == null)
            {
                return false;
            }
            return DataValidation.ValidateName(Product.Name)
              && DataValidation.ValidateCategory(Product.Category)
              && DataValidation.ValidatePrice(Product.Price)
              && DataValidation.ValidateQuantity(Product.Quantity)
              && DataValidation.ValidateDescription(Product.Description)
              && DataValidation.ValidateImageURL(Product.ImageURL);
        }
    }

    public class ProductFilter
    {
        public string category { get; set; } = "";
        public decimal low { get; set; } = 0;
        public decimal high { get; set; } = decimal.MaxValue;
        public bool Availability { get; set; } = false;
    }
}