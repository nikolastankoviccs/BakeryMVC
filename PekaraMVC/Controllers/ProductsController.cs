using Newtonsoft.Json;
using PekaraMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PekaraMVC.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            var products = Details();
            return View(products);
        }

        public List<Product> Details()
        {
            string jsonFromFile= System.IO.File.ReadAllText("Products.json");
            List<Product> productsFromFile = JsonConvert.DeserializeObject<List<Product>>(jsonFromFile);

            return productsFromFile;

        }
    }
}