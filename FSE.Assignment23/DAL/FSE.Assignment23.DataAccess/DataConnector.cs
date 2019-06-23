using System;
using System.Collections.Generic;
using System.Linq;

namespace FSE.Assignment23.DataAccess
{
    public class DataConnector
    {
        private DataConnector() { }
        private static readonly Lazy<DataConnector> lazy = new Lazy<DataConnector>(() => new DataConnector());
        public static DataConnector Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        #region Suppliers
        public List<SUPPLIER> GetSuppliers()
        {
            return new PODbModel().SUPPLIERs.ToList();
        }

        public SUPPLIER GetSupplierByNumber(string code)
        {
            return new PODbModel().SUPPLIERs.FirstOrDefault(x => x.SUPLNO == code);
        }

        public void CreateSupplier(SUPPLIER supplier)
        {
            var model = new PODbModel();
            model.SUPPLIERs.Add(supplier);
            model.SaveChanges();
        }
        public void UpdateSupplier(SUPPLIER supplier)
        {
            var model = new PODbModel();
            var oldSupplier = model.SUPPLIERs.FirstOrDefault(x => x.SUPLNO == supplier.SUPLNO);
            if (oldSupplier != null)
            {
                oldSupplier.SUPLNAME = supplier.SUPLNAME;
                oldSupplier.SUPLADDR = supplier.SUPLADDR;
                model.SaveChanges();
            }
        }
        public void DeleteSupplier(string code)
        {
            var model = new PODbModel();
            var supplier = model.SUPPLIERs.FirstOrDefault(x => x.SUPLNO == code);
            if (supplier != null)
            {
                model.SUPPLIERs.Remove(supplier);
                model.SaveChanges();
            }
        }
        #endregion Suppliers
        #region Items
        public List<ITEM> GetItems()
        {
            return new PODbModel().ITEMs.ToList();
        }

        public ITEM GetItemByNumber(string code)
        {
            return new PODbModel().ITEMs.FirstOrDefault(x => x.ITCODE == code);
        }
        public void CreateItem(ITEM item)
        {
            var model = new PODbModel();
            model.ITEMs.Add(item);
            model.SaveChanges();
        }
        public void UpdateItem(ITEM item)
        {
            var model = new PODbModel();
            var oldItem = model.ITEMs.FirstOrDefault(x => x.ITCODE == item.ITCODE);
            if (oldItem != null)
            {
                oldItem.ITDESC = item.ITDESC;
                oldItem.ITRATE = item.ITRATE;
                model.SaveChanges();
            }
        }
        public void DeleteItem(string code)
        {
            var model = new PODbModel();
            var item = model.ITEMs.FirstOrDefault(x => x.ITCODE == code);
            if (item != null)
            {
                model.ITEMs.Remove(item);
                model.SaveChanges();
            }
        }
        #endregion Items
    }
}
