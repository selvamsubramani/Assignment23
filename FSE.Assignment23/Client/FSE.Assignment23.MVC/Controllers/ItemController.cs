using FSE.Assignment23.MVC.Models;
using FSE.Assignment23.MVC.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FSE.Assignment23.MVC.Controllers
{
    public class ItemController : Controller
    {
        private readonly IWebApiClient _client;
        private const string API = "Item";

        public ItemController(IWebApiClient client)
        {
            _client = client;
        }
        public ItemController() : this(new WebApiClient()) { }

        public ActionResult Index()
        {
            IEnumerable<Item> items = null;
            var result = _client.GetData(API);
            if (result != null)
            {
                items = JsonConvert.DeserializeObject<IEnumerable<Item>>(result);
            }
            return View(items);
        }

        public ActionResult Create()
        {
            return View(new Item());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var item = new Item
            {
                Code = collection.Get("Code"),
                Description = collection.Get("Description"),
                Rate = Convert.ToDecimal(collection.Get("Rate"))
            };

            var result = _client.AddData(API, JsonConvert.SerializeObject(item));
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(item);
            }
        }

        public ActionResult Edit(string code)
        {
            Item item = null;
            var result = _client.GetData(API, code);
            if (result != null)
            {
                item = JsonConvert.DeserializeObject<Item>(result);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(string code, FormCollection collection)
        {
            var item = new Item
            {
                Code = code,
                Description = collection.Get("Description"),
                Rate = Convert.ToDecimal(collection.Get("Rate"))
            };

            var result = _client.EditData(API, JsonConvert.SerializeObject(item));

            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(item);
            }
        }

        public ActionResult Delete(string code)
        {
            _client.DeleteData(API, code);
            return RedirectToAction("Index");
        }
    }
}
