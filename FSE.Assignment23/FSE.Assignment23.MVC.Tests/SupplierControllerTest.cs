using FSE.Assignment23.MVC.Controllers;
using FSE.Assignment23.MVC.Models;
using FSE.Assignment23.MVC.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FSE.Assignment23.MVC.Tests
{
    [TestClass]
    public class SupplierControllerTest
    {
        private IWebApiClient client = null;

        List<Supplier> suppliers = new List<Supplier>
            {
                new Supplier{ Code="S001", Name="A2Z", Address="USA"},
                new Supplier{ Code="S002", Name="ABC", Address="UK"}
            };

        [TestInitialize]
        public void Setup()
        {
            var mockClient = new Mock<IWebApiClient>();

            mockClient.Setup(method =>
                method.GetData(It.IsAny<string>(), null))
                .Returns(JsonConvert.SerializeObject(suppliers));

            mockClient.Setup(method =>
                method.GetData(It.IsAny<string>(), It.Is<string>(x => x != null)))
                .Returns<string, string>((name, data) =>
                {
                    var supplier = suppliers.FirstOrDefault(x => x.Code == data);
                    if (supplier != null)
                        return JsonConvert.SerializeObject(supplier);
                    return null;
                });

            mockClient.Setup(method =>
                method.AddData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((name, data) =>
                 {
                     try
                     {
                         var supplier = JsonConvert.DeserializeObject<Supplier>(data);
                         suppliers.Add(supplier);
                         return true;
                     }
                     catch (Exception)
                     {
                         return false;
                     }
                 });

            mockClient.Setup(method =>
                method.EditData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((name, data) =>
                {
                    try
                    {
                        var supplier = JsonConvert.DeserializeObject<Supplier>(data);
                        var edited = suppliers.FirstOrDefault(x => x.Code == supplier.Code);
                        if (edited != null)
                        {
                            edited.Name = supplier.Name;
                            edited.Address = supplier.Address;
                            return true;
                        }
                        return false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
            mockClient.Setup(method =>
                method.DeleteData(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((name, data) =>
                {
                    var supplier = suppliers.FirstOrDefault(x => x.Code == data);
                    if (supplier != null)
                        suppliers.Remove(supplier);
                })
                .Verifiable("Failed");

            client = mockClient.Object;
        }

        [TestMethod]
        public void ShouldStartIndex()
        {
            Console.WriteLine("Initalizing Supplier View");
            var controller = new SupplierController(client);
            var result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var view = result as ViewResult;
            var suppliers = view.ViewData.Model as List<Supplier>;
            Assert.AreEqual(2, suppliers.Count);
            Assert.IsNotNull(suppliers);
            foreach (var supplier in suppliers)
            {
                Console.WriteLine($"Code-{supplier.Code} Name-{supplier.Name} Address-{supplier.Address}");
            }
        }

        [TestMethod]
        public void ShouldLaunchCreateView()
        {
            Console.WriteLine("Initalizing Supplier Creation View");
            var controller = new SupplierController(client);
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ShouldCreateSupplier()
        {
            Console.WriteLine("Initalizing Supplier Add View");
            var controller = new SupplierController(client);
            var collection = new FormCollection
            {
                ["Code"] = "S003",
                ["Name"] = "Test-Supplier",
                ["Address"] = "India"
            };
            var currentSupplierCount = suppliers.Count;
            Console.WriteLine($"Before adding supplier: No of suppliers-{currentSupplierCount}");
            var result = controller.Create(collection);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(currentSupplierCount + 1, suppliers.Count);
            Console.WriteLine($"After adding supplier: No of suppliers-{suppliers.Count}");
            var added = suppliers.FirstOrDefault(x => x.Code == "S003");
            Assert.IsNotNull(added);
            Console.WriteLine($"Code-{added.Code} Name-{added.Name} Address-{added.Address}");
        }
        [TestMethod]
        public void ShouldLaunchEditView()
        {
            Console.WriteLine("Initalizing Supplier Edit View");
            var controller = new SupplierController(client);
            var result = controller.Edit("S001");
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var supplier = view.ViewData.Model as Supplier;
            Assert.IsNotNull(supplier);
            Console.WriteLine($"Code-{supplier.Code} Name-{supplier.Name} Address-{supplier.Address}");
        }
        [TestMethod]
        public void ShouldUpdateSupplier()
        {
            Console.WriteLine("Initalizing Supplier Update View");
            var controller = new SupplierController(client);
            var collection = new FormCollection
            {
                ["Code"] = "S001",
                ["Name"] = "Supplier-Edited",
                ["Address"] = "India"
            };
            Console.Write($"Before supplier edit: ");
            var supplier = suppliers.FirstOrDefault(x => x.Code == "S001");
            Assert.IsNotNull(supplier);
            Console.WriteLine($"Code-{supplier.Code} Name-{supplier.Name} Address-{supplier.Address}");
            var result = controller.Edit("S001", collection);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Console.Write($"After supplier edit: ");
            supplier = suppliers.FirstOrDefault(x => x.Code == "S001");
            Assert.IsNotNull(supplier);
            Console.WriteLine($"Code-{supplier.Code} Name-{supplier.Name} Address-{supplier.Address}");
            var expectedSupplier = "S001-Supplier-Edited-India";
            var actualSupplier = $"{supplier.Code}-{supplier.Name}-{supplier.Address}";
            Assert.AreEqual(expectedSupplier, actualSupplier);
        }

        [TestMethod]
        public void ShouldDeleteSupplier()
        {
            Console.WriteLine("Initalizing Supplier Delete View");
            var controller = new SupplierController(client);
            var currentSupplierCount = suppliers.Count;
            Console.WriteLine($"Before deleting supplier: No of suppliers-{currentSupplierCount}");
            var supplier = suppliers.FirstOrDefault(x => x.Code == "S001");
            Assert.IsNotNull(supplier);
            Console.WriteLine($"Supplier to delete: Code-{supplier.Code} Name-{supplier.Name} Address-{supplier.Address}");
            var result = controller.Delete("S001");
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(currentSupplierCount - 1, suppliers.Count);
            Console.WriteLine($"After deleting supplier: No of suppliers-{suppliers.Count}");
            var added = suppliers.FirstOrDefault(x => x.Code == "S001");
            Assert.IsNull(added);
        }
    }
}