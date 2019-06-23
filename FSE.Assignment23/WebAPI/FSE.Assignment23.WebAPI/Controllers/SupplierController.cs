using FSE.Assignment23.DataAccess;
using FSE.Assignment23.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FSE.Assignment23.WebAPI.Controllers
{
    public class SupplierController : ApiController
    {
        // GET: api/Supplier
        public IEnumerable<Supplier> Get()
        {
            return
                DataConnector.Instance.GetSuppliers().Select(x =>
                new Supplier
                {
                    Code = x.SUPLNO,
                    Name = x.SUPLNAME,
                    Address = x.SUPLADDR
                });
        }

        // GET: api/Supplier/5
        public Supplier Get(string code)
        {
            var result = DataConnector.Instance.GetSupplierByNumber(code);
            if (result != null)
            {
                return
                    new Supplier
                    {
                        Code = result.SUPLNO,
                        Name = result.SUPLNAME,
                        Address = result.SUPLADDR
                    };
            }
            return null;
        }


        // POST: api/Supplier
        public void Post(Supplier supplier)
        {
            DataConnector.Instance.CreateSupplier(
                new SUPPLIER
                {
                    SUPLNO = supplier.Code,
                    SUPLNAME = supplier.Name,
                    SUPLADDR = supplier.Address
                });

        }

        // PUT: api/Supplier/5
        public void Put(string code, Supplier supplier)
        {
            DataConnector.Instance.UpdateSupplier(
                new SUPPLIER
                {
                    SUPLNO = code,
                    SUPLNAME = supplier.Name,
                    SUPLADDR = supplier.Address
                });
        }

        // DELETE: api/Supplier/5
        public void Delete(string code)
        {
            DataConnector.Instance.DeleteSupplier(code);
        }
    }
}
