using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System;

namespace Fujitsu.CvQc.Console.App
{
    public class DataService : IDataService
    {
        private IConfiguration configuration = ServiceInjector.GetService<IConfiguration>();

        public T? GetSync<T>(string url)
        {
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var task = client.GetStringAsync(GetUrl(url));
                Task.WaitAll(task);

                string response = task.Result;
                var deserializedObject = JsonConvert.DeserializeObject<T>(response);

                return deserializedObject;
            }
        }

        public bool PostSync<T>(string url, T payload)
        {
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serializedObject = JsonConvert.SerializeObject(payload);
                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                var task = client.PostAsync(GetUrl(url), content);
                Task.WaitAll(task);

                return task.Result.StatusCode == HttpStatusCode.OK;
            }
        }

        public bool PutSync<T>(string url, T payload)
        {
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var serializedObject = JsonConvert.SerializeObject(payload);
                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                var task = client.PutAsync(GetUrl(url), content);
                Task.WaitAll(task);

                return task.Result.StatusCode == HttpStatusCode.OK;
            }
        }

        public bool DeleteSync(string url)
        {
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var task = client.DeleteAsync(GetUrl(url));
                Task.WaitAll(task);

                return task.Result.StatusCode == HttpStatusCode.OK;
            }
        }

        private string GetUrl(string url)
        {
            var baseUrl = configuration["BaseUrl"];
            System.Console.WriteLine(baseUrl + url);
            return baseUrl + url;
        }
    }
}
