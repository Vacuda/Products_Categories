using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prod_Cate.Models;

namespace Prod_Cate.Controllers
{
    [Route("Products")]
    public class ProductController : Controller
    {
        private HomeContext dbContext;
     
        public ProductController(HomeContext context)
        {
            dbContext = context;
        }

        public IActionResult Privacy(){
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(){
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        ////////////pages

        [HttpGet("")]
        public IActionResult Products_Main_Page(){
            ViewBag.Products = dbContext.Products;
            return View("Products_Main_Page");
        }

        [HttpGet("View/{id}")]
        public IActionResult Products_View_Page(int id){
            ViewBag.Product = GetFullProduct(id);
            return View("Products_View_Page", GetNonListCat(id));
        }
    

        [HttpGet("Edit/{id}")]
        public IActionResult Products_Edit_Page(int id){
            return View("Products_Edit_Page", GetFullProduct(id));
        }

        ////////////actions

        [HttpPost("Create_Product")]
        public IActionResult Create_Product(Product product){
            if (ModelState.IsValid){
                dbContext.Products.Add(product);
                dbContext.SaveChanges();
                return RedirectToAction("Products_Main_Page");
            }
            else {
                ViewBag.Products = dbContext.Products;
                return View("Products_Main_Page", product);
            }
        }

        [HttpPost("Add_Cat")]
        public IActionResult Add_Cat(){
            int cat_id = Convert.ToInt32(Request.Form["Category"]);
            int prod_id = Convert.ToInt32(Request.Form["Product"]);
            Connection new_connection = new Connection(){
                ProductId = prod_id,
                CategoryId = cat_id,
            };
            dbContext.Connections.Add(new_connection);
            dbContext.SaveChanges();

            return Redirect($"/Products/View/{prod_id}");
        }

        [HttpGet("Remove_Cat/{prod_id}/{cat_id}")]
        public IActionResult Remove_Cat(int prod_id, int cat_id){

            Connection to_sever = dbContext.Connections.Where(e=>e.CategoryId == cat_id)
                                                    .Where(e=>e.ProductId == prod_id)
                                                    .First();

            dbContext.Connections.Remove(to_sever);
            dbContext.SaveChanges();

            return Redirect($"/Products/View/{prod_id}");

        }

        [HttpPost("Edit_Product/{id}")]
        public IActionResult Edit_Product(Product product, int id){
            if(ModelState.IsValid){
                //update object
                Product to_update = GetFullProduct(id);
                to_update.Name = product.Name;
                to_update.Description = product.Description;
                to_update.Price = product.Price;
                to_update.Updated_At = DateTime.Now;
                dbContext.SaveChanges();

                return Redirect($"/Products/View/{id}");
            }
            return View("Products_Edit_Page", GetFullProduct(id));
        }

       
        [HttpGet("Delete/{id}")]
        public IActionResult Delete_Product(int id){
            Product to_delete = GetFullProduct(id);
            dbContext.Products.Remove(to_delete);
            dbContext.SaveChanges();
            return Redirect("/Products");
        }




        //Functions

        public Product GetFullProduct(int id){
            Product product = dbContext.Products.Include(e=>e.Connections)
                                    .ThenInclude(e=>e.Category)
                                    .FirstOrDefault(e=>e.ProductId == id);
            return product;
        }

        public List<Category> GetNonListCat(int id){
            List<Category> non_cat_list = dbContext.Categories.Include(e=>e.Connections)
                                    .Where(e=>e.Connections.All(f=>f.ProductId != id))
                                    .ToList();
            return non_cat_list;
        }











    }
}