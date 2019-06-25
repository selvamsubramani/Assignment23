using FSE.Assignment23.MVC.Models;
using FSE.Assignment23.MVC.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FSE.Assignment23.MVC.Controllers
{
    public class SupplierController : Controller
    {
        private readonly IWebApiClient _client;
        private const string API = "Supplier";

        public SupplierController(IWebApiClient client)
        {
            _client = client;
        }
        public SupplierController() : this(new WebApiClient()) { }

        public ActionResult Index()
        {
            IEnumerable<Supplier> suppliers = null;
            var result = _client.GetData(API);
            if (result != null)
            {
                suppliers = JsonConvert.DeserializeObject<IEnumerable<Supplier>>(result);
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

            var result = _client.AddData(API, JsonConvert.SerializeObject(supplier));
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(supplier);
            }
        }
        public ActionResult Edit(string code)
        {
            Supplier supplier = null;
            var result = _client.GetData(API, code);
            if (result != null)
            {
                supplier = JsonConvert.DeserializeObject<Supplier>(result);
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
            var result = _client.EditData(API, JsonConvert.SerializeObject(supplier));

            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(supplier);
            }
        }
        public ActionResult Delete(string code)
        {
            _client.DeleteData(API, code);
            return RedirectToAction("Index");
        }
    }
}