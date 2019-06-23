using FSE.Assignment23.MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;

namespace FSE.Assignment23.MVC.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier
        public ActionResult Index()
        {
            IEnumerable<Supplier> suppliers = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60394/api/");
                var task = client.GetAsync("Supplier");
                task.Wait();
                var result = task.Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync();
                    response.Wait();
                    suppliers = JsonConvert.DeserializeObject<IEnumerable<Supplier>>(response.Result);
                }
            }
            return View(suppliers);
        }
        public ActionResult Create()
        {
            return View(new Supplier());
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var supplier = new Supplier
            {
                Code = collection.Get("Code"),
                Name = collection.Get("Name"),
                Address = collection.Get("Address")
            };

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri("http://localhost:60394/api/");

                var task = client.PostAsync("Supplier", content);
                task.Wait();
                var result = task.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(supplier);
                }
            }
        }
        public ActionResult Edit(string code)
        {
            Supplier supplier = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60394/api/");
                var task = client.GetAsync($"Supplier/{code}");
                task.Wait();
                var result = task.Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync();
                    response.Wait();
                    supplier = JsonConvert.DeserializeObject<Supplier>(response.Result);
                }
            }
            return View(supplier);
        }
        [HttpPost]
        public ActionResult Edit(string code, FormCollection collection)
        {

            var supplier = new Supplier
            {
                Code = code,
                Name = collection.Get("Name"),
                Address = collection.Get("Address")
            };

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri("http://localhost:60394/api/");

                var task = client.PutAsync($"Supplier/{code}", content);
                task.Wait();
                var result = task.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(supplier);
                }
            }
        }
        public ActionResult Delete(string code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60394/api/");
                var task = client.DeleteAsync($"Supplier/{code}");
                task.Wait();
                if (task.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}