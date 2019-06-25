using System;
using System.Net.Http;
using System.Text;

namespace FSE.Assignment23.MVC.Utility
{
    public class WebApiClient : IWebApiClient
    {
        private const string API = "http://localhost:60394/api/";

        public bool AddData(string method, string data)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(API);
                var task = client.PostAsync(method, content);
                task.Wait();
                var result = task.Result;
                return result.IsSuccessStatusCode;
            }
        }

        public void DeleteData(string method, string data)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(API);
                var task = client.DeleteAsync($"{method}/{data}");
                task.Wait();
            }
        }

        public bool EditData(string method, string data)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(API);
                var task = client.PutAsync(method, content);
                task.Wait();
                var result = task.Result;
                return result.IsSuccessStatusCode;
            }
        }

        public string GetData(string method, string data=null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API);
                if (data != null)
                    method = $"{method}/{data}";
                var task = client.GetAsync(method);
                task.Wait();
                var result = task.Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync();
                    response.Wait();
                    return response.Result;
                }
                return null;
            }
        }
    }
}