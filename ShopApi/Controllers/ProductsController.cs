using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopApi.Models;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ShopDBContext _shopDbContext;

        public ProductsController(ShopDBContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var products = await _shopDbContext.Product.ToListAsync();

            return new JsonResult(products);
        }

        [HttpGet]
        [Route("{productId:int}")]
        public async Task<ActionResult> GetById(int productId)
        {
            var product = await _shopDbContext.Product.FindAsync(productId);

            if (product == null)
                return new NotFoundResult();

            return new JsonResult(product);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Product product)
        {
            var dbProduct = await _shopDbContext.Product.FindAsync(product.Id);
            if (dbProduct != null)
                return Conflict();

            product.CreationDate = DateTime.Now;

            await _shopDbContext.Product.AddAsync(product);
            await _shopDbContext.SaveChangesAsync();

            return Created(new Uri($"https://localhost:44328/products/{product.Id}"), product);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Product product)
        {
            var dbProduct = await _shopDbContext.Product.FindAsync(product.Id);
            if (dbProduct == null)
                return NotFound();

            dbProduct.UpdateDate = DateTime.Now;
            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.Stock = product.Stock;

            _shopDbContext.Product.Update(dbProduct);
            await _shopDbContext.SaveChangesAsync();

            return Ok(dbProduct);
        }

        [HttpGet]
        [Route("customers-total-spent")]
        public async Task<ActionResult> GetTotalSpentByCustomers()
        {
            var orders = await _shopDbContext.Order
                .Include("Customer")
                .Include("OrderItem.Product")
                .ToListAsync();

            var customerOrders = orders.GroupBy(x => x.Customer.Email)
                .ToDictionary(x => x.Key, x => x.SelectMany(o => o.OrderItem.Select(oi => oi.Product.Price)).Sum());

            return new JsonResult(customerOrders.Select(x => new { Customer = x.Key, TotalSpent = x.Value }));
        }
    }
}
