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
    public class HomeController : Controller
    {
        private HomeContext dbContext;
     
        public HomeController(HomeContext context)
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

        [HttpGet("")]
        public IActionResult Index(){
            return RedirectToAction ("Products_Main_Page", "Product");
        }

    }
}
