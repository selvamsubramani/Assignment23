namespace FSE.Assignment23.MVC.Utility
{
    public interface IWebApiClient
    {
        string GetData(string method, string data = null);
        bool AddData(string method, string data);
        bool EditData(string method, string data);
        void DeleteData(string method, string data);
    }
}
