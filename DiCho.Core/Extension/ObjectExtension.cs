using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.Core.Extension
{
    public static class ObjectExtension
    {
        public static HttpContent ToHttpContent(this object obj, JsonSerializerSettings jsonSerializerSettings)
        {
            return new StringContent(JsonConvert.SerializeObject(obj, jsonSerializerSettings), Encoding.UTF8, "application/json");
        }
    }
}
