using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prod_Cate.Models;

namespace Prod_Cate.Controllers
{
    [Route("Categories")]
    public class CategoryController : Controller
    {
        private HomeContext dbContext;
     
        public CategoryController(HomeContext context)
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
        public IActionResult Categories_Main_Page(){
            ViewBag.Categories = dbContext.Categories;
            return View("Categories_Main_Page");
        }

        [HttpGet("View/{id}")]
        public IActionResult Categories_View_Page(int id){
            System.Console.WriteLine($"((((((((((((((((((({id}))))))))))))))))");
            ViewBag.Category = GetFullCategory(id);
            Category c = GetFullCategory(id);
            System.Console.WriteLine($"((((((((((((((((((({c.Name}))))))))))))))))");
            return View("Categories_View_Page", GetNonListProd(id));
        }
    

        [HttpGet("Edit/{id}")]
        public IActionResult Categories_Edit_Page(int id){
            return View("Categories_Edit_Page", GetFullCategory(id));
        }

        ////////////actions

       

        [HttpPost("Create_Category")]
        public IActionResult Create_Category(Category category){
            if (ModelState.IsValid){
                dbContext.Categories.Add(category);
                dbContext.SaveChanges();
                return RedirectToAction("Categories_Main_Page");
            }
            else {
                ViewBag.Categories = dbContext.Categories;
                return View("Categories_Main_Page", category);
            }
        }

        [HttpPost("Add_Prod")]
        public IActionResult Add_Prod(){
            int cat_id = Convert.ToInt32(Request.Form["Category"]);
            int prod_id = Convert.ToInt32(Request.Form["Product"]);
            Connection new_connection = new Connection(){
                ProductId = prod_id,
                CategoryId = cat_id,
            };
            dbContext.Connections.Add(new_connection);
            dbContext.SaveChanges();

            return Redirect($"/Categories/View/{cat_id}");
        }


        [HttpGet("Remove_Prod/{prod_id}/{cat_id}")]
        public IActionResult Remove_Prod(int prod_id, int cat_id){

            Connection to_sever = dbContext.Connections.Where(e=>e.CategoryId == cat_id)
                                                    .Where(e=>e.ProductId == prod_id)
                                                    .First();

            dbContext.Connections.Remove(to_sever);
            dbContext.SaveChanges();

            return Redirect($"/Categories/View/{cat_id}");
        }

        [HttpPost("Edit_Category/{id}")]
        public IActionResult Edit_Category(Category category, int id){
            if(ModelState.IsValid){
                //update object
                Category to_update = GetFullCategory(id);
                to_update.Name = category.Name;
                to_update.Updated_At = DateTime.Now;
                dbContext.SaveChanges();

                return Redirect($"/Categories/View/{id}");
            }
            return View("Categories_Edit_Page", GetFullCategory(id));
        }

        [HttpGet("Delete/{id}")]
        public IActionResult Delete_Category(int id){
            Category to_delete = GetFullCategory(id);
            dbContext.Categories.Remove(to_delete);
            dbContext.SaveChanges();
            return Redirect("/Categories");
        }


        //Functions

        public Category GetFullCategory(int id){
            Category category = dbContext.Categories.Include(e=>e.Connections)
                                    .ThenInclude(e=>e.Product)
                                    .FirstOrDefault(e=>e.CategoryId == id);
            return category;
        }

        public List<Product> GetNonListProd(int id){
            List<Product> non_prod_list = dbContext.Products.Include(e=>e.Connections)
                                    .Where(e=>e.Connections.All(f=>f.CategoryId != id))
                                    .ToList();
            return non_prod_list;
        }

















    }
}