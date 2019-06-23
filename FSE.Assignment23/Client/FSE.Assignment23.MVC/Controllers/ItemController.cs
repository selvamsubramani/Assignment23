using FSE.Assignment23.MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;

namespace FSE.Assignment23.MVC.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Index()
        {
            IEnumerable<Item> items = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60394/api/");
                var task = client.GetAsync("Item");
                task.Wait();
                var result = task.Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync();
                    response.Wait();
                    items = JsonConvert.DeserializeObject<IEnumerable<Item>>(response.Result);
                }
            }
            return View(items);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            return View(new Item());
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var item = new Item
            {
                Code = collection.Get("Code"),
                Description = collection.Get("Description"),
                Rate = Convert.ToDecimal(collection.Get("Rate"))
            };

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri("http://localhost:60394/api/");

                var task = client.PostAsync("Item", content);
                task.Wait();
                var result = task.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(item);
                }
            }
        }

        // GET: Item/Edit/5
        public ActionResult Edit(string code)
        {
            Item item = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60394/api/");
                var task = client.GetAsync($"Item/{code}");
                task.Wait();
                var result = task.Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync();
                    response.Wait();
                    item = JsonConvert.DeserializeObject<Item>(response.Result);
                }
            }
            return View(item);
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(string code, FormCollection collection)
        {
            var item = new Item
            {
                Code = code,
                Description = collection.Get("Description"),
                Rate = Convert.ToDecimal(collection.Get("Rate"))
            };

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri("http://localhost:60394/api/");

                var task = client.PutAsync($"Item/{code}", content);
                task.Wait();
                var result = task.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(item);
                }
            }
        }

        // GET: Item/Delete/5
        public ActionResult Delete(string code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60394/api/");
                var task = client.DeleteAsync($"Item/{code}");
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
