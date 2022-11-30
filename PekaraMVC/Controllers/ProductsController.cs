using Newtonsoft.Json;
using PekaraMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            string jsonFromFile = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/Products.json"));
            List<Product> listFromFile = JsonConvert.DeserializeObject<List<Product>>(jsonFromFile);

            return listFromFile;
        }

        
        public ActionResult Update(int id)
        {
            var product = Details().FirstOrDefault(q => q.Id == id);
            
            return View();
        }

        public ActionResult GetProduct(int id)
        {
            var product = Details().SingleOrDefault(q => q.Id == id);
            return View(product);
        }

        public ActionResult CreateNew()
        {
            

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Price")] Product redefProduct)
        {
            if(ModelState.IsValid)
            {
                List<Product> products = Details();
                Product product = products.SingleOrDefault(c => c.Id == redefProduct.Id);
                products.Remove(product);
                products.Add(redefProduct);
                RefreshList(products);
                return RedirectToAction("Index");
            }

            return View(redefProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create ([Bind(Include ="Name,Price")] Product product)
        {
            if(ModelState.IsValid)
            {
                AddProduct(product);
                return RedirectToAction("Index");
            }

            return View(product);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = Details().SingleOrDefault(q => q.Id == id);
            if(product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List <Product> productList = Details();
            Product product = productList.SingleOrDefault(q=>q.Id==id);
            productList.Remove(product);
            RefreshList(productList);
            return RedirectToAction("Indext");
        }
        public void AddProduct(Product product)
        {
            var list = Details();
            product.Id = list.Count;
            list.Add(product);
            string jsonToFile = JsonConvert.SerializeObject(list, Formatting.Indented);
            System.IO.File.WriteAllText(Server.MapPath(@"~/App_Data/Products.json"), jsonToFile);
        }

        public void RefreshList(List<Product> productList)
        {
            System.IO.File.WriteAllText(Server.MapPath(@"~/App_Data/Products.json"), "");
            string jsonToFile = JsonConvert.SerializeObject(productList, Formatting.Indented);
            System.IO.File.WriteAllText(Server.MapPath(@"~/App_Data/Products.json"), jsonToFile);
        }
    }
}