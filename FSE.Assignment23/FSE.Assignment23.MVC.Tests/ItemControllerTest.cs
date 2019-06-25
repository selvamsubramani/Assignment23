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
    public class ItemControllerTest
    {
        private IWebApiClient client = null;

        List<Item> items = new List<Item>
            {
                new Item{ Code="I001", Description ="AC", Rate=50000},
                new Item{ Code="I002", Description="TV", Rate=40000}
            };

        [TestInitialize]
        public void Setup()
        {
            var mockClient = new Mock<IWebApiClient>();

            mockClient.Setup(method =>
                method.GetData(It.IsAny<string>(), null))
                .Returns(JsonConvert.SerializeObject(items));

            mockClient.Setup(method =>
                method.GetData(It.IsAny<string>(), It.Is<string>(x => x != null)))
                .Returns<string, string>((name, data) =>
                {
                    var item = items.FirstOrDefault(x => x.Code == data);
                    if (item != null)
                        return JsonConvert.SerializeObject(item);
                    return null;
                });

            mockClient.Setup(method =>
                method.AddData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((name, data) =>
                {
                    try
                    {
                        var item = JsonConvert.DeserializeObject<Item>(data);
                        items.Add(item);
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
                        var item = JsonConvert.DeserializeObject<Item>(data);
                        var edited = items.FirstOrDefault(x => x.Code == item.Code);
                        if (edited != null)
                        {
                            edited.Description = item.Description;
                            edited.Rate = item.Rate;
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
                    var item = items.FirstOrDefault(x => x.Code == data);
                    if (item != null)
                        items.Remove(item);
                })
                .Verifiable("Failed");

            client = mockClient.Object;
        }

        [TestMethod]
        public void ShouldStartIndex()
        {
            Console.WriteLine("Initalizing Item View");
            var controller = new ItemController(client);
            var result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var view = result as ViewResult;
            var items = view.ViewData.Model as List<Item>;
            Assert.AreEqual(2, items.Count);
            Assert.IsNotNull(items);
            foreach (var item in items)
            {
                Console.WriteLine($"Code-{item.Code} Description-{item.Description} Rate-{item.Rate}");
            }
        }
        [TestMethod]
        public void ShouldLaunchCreateView()
        {
            Console.WriteLine("Initalizing Item Creation View");
            var controller = new ItemController(client);
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ShouldCreateItem()
        {
            Console.WriteLine("Initalizing Item Add View");
            var controller = new ItemController(client);
            var collection = new FormCollection
            {
                ["Code"] = "I003",
                ["Description"] = "Test-Item",
                ["Rate"] = "25000"
            };
            var currentItemCount = items.Count;
            Console.WriteLine($"Before adding item: No of items-{currentItemCount}");
            var result = controller.Create(collection);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(currentItemCount + 1, items.Count);
            Console.WriteLine($"After adding item: No of items-{items.Count}");
            var added = items.FirstOrDefault(x => x.Code == "I003");
            Assert.IsNotNull(added);
            Console.WriteLine($"Code-{added.Code} Description-{added.Description} Rate-{added.Rate}");
        }
        [TestMethod]
        public void ShouldLaunchEditView()
        {
            Console.WriteLine("Initalizing Item Edit View");
            var controller = new ItemController(client);
            var result = controller.Edit("I001");
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var item = view.ViewData.Model as Item;
            Assert.IsNotNull(item);
            Console.WriteLine($"Code-{item.Code} Description-{item.Description} Rate-{item.Rate}");
        }
        [TestMethod]
        public void ShouldUpdateItem()
        {
            Console.WriteLine("Initalizing Item Update View");
            var controller = new ItemController(client);
            var collection = new FormCollection
            {
                ["Code"] = "I001",
                ["Description"] = "Item-Edited",
                ["Rate"] = "10000"
            };
            Console.Write($"Before item edit: ");
            var item = items.FirstOrDefault(x => x.Code == "I001");
            Assert.IsNotNull(item);
            Console.WriteLine($"Code-{item.Code} Description-{item.Description} Rate-{item.Rate}");
            var result = controller.Edit("I001", collection);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Console.Write($"After item edit: ");
            item = items.FirstOrDefault(x => x.Code == "I001");
            Assert.IsNotNull(item);
            Console.WriteLine($"Code-{item.Code} Description-{item.Description} Rate-{item.Rate}");
            var expectedItem = "I001-Item-Edited-10000.0";
            var actualItem = $"{item.Code}-{item.Description}-{item.Rate}";
            Assert.AreEqual(expectedItem, actualItem);
        }

        [TestMethod]
        public void ShouldDeleteItem()
        {
            Console.WriteLine("Initalizing Item Delete View");
            var controller = new ItemController(client);
            var currentItemCount = items.Count;
            Console.WriteLine($"Before deleting item: No of items-{currentItemCount}");
            var item = items.FirstOrDefault(x => x.Code == "I001");
            Assert.IsNotNull(item);
            Console.WriteLine($"Item to delete: Code-{item.Code} Description-{item.Description} Rate-{item.Rate}");
            var result = controller.Delete("I001");
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual(currentItemCount - 1, items.Count);
            Console.WriteLine($"After deleting item: No of items-{items.Count}");
            var added = items.FirstOrDefault(x => x.Code == "I001");
            Assert.IsNull(added);
        }
    }
}
