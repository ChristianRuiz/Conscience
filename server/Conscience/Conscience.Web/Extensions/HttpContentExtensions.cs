using Conscience.DataAccess;
using Conscience.Web.Identity;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Conscience.Web
{
    public static class HttpContentExtensions
    {
        public static async Task<string> ReadToEndAsync(this HttpContent content)
        {
            var stream = await content.ReadAsStreamAsync();
            stream.Position = 0;
            return new StreamReader(stream).ReadToEnd();
        }

        public static async Task<T> DeserializeAsync<T>(this HttpContent content)
        {
            var json = await content.ReadToEndAsync();
            var query = JsonConvert.DeserializeObject<T>(json);
            return query;
        }

    }
}