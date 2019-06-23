using FSE.Assignment23.DataAccess;
using FSE.Assignment23.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FSE.Assignment23.WebAPI.Controllers
{
    public class ItemController : ApiController
    {
        // GET: api/Item
        public IEnumerable<Item> Get()
        {
            return
                DataConnector.Instance.GetItems().Select(x =>
                new Item
                {
                    Code = x.ITCODE,
                    Description = x.ITDESC,
                    Rate = x.ITRATE.Value
                });
        }

        // GET: api/Item/5
        public Item Get(string code)
        {
            var result = DataConnector.Instance.GetItemByNumber(code);
            if (result != null)
            {
                return
                    new Item
                    {
                        Code = result.ITCODE,
                        Description = result.ITDESC,
                        Rate = result.ITRATE.Value
                    };
            }
            return null;
        }

        // POST: api/Item
        public void Post(Item item)
        {
            DataConnector.Instance.CreateItem(
               new ITEM
               {
                   ITCODE = item.Code,
                   ITDESC = item.Description,
                   ITRATE = item.Rate
               });
        }

        // PUT: api/Item/5
        public void Put(string code, Item item)
        {
            DataConnector.Instance.UpdateItem(
               new ITEM
               {
                   ITCODE = code,
                   ITDESC = item.Description,
                   ITRATE = item.Rate
               });
        }

        // DELETE: api/Item/5
        public void Delete(string code)
        {
            DataConnector.Instance.DeleteItem(code);
        }
    }
}
