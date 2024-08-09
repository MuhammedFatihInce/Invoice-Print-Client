using Newtonsoft.Json;
using partsSoftClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace partsSoftClient.Services
{
    public class ApiService<TEntity, TUpdate>
        where TEntity : class, new()
        where TUpdate : class, new()
    {
        
        public List<TEntity> Get(string url, string endPoint)
        {
            List<TEntity> entity;

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = client.GetAsync(endPoint).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                entity = JsonConvert.DeserializeObject<List<TEntity>>(result);
            }

            return entity;
        }

        
        public ResponseModel Post(TEntity entity, string postUrl)
        {
            var json = JsonConvert.SerializeObject(entity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = postUrl;

            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url, data).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                var responseModel = JsonConvert.DeserializeObject<ResponseModel>(result);

                return responseModel;
            }
        }

        
        public ResponseModel Update(TUpdate entity, string updateUrl)
        {
            var json = JsonConvert.SerializeObject(entity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = updateUrl;

            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url, data).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                var responseModel = JsonConvert.DeserializeObject<ResponseModel>(result);

                return responseModel;
            }
        }
    }
}
