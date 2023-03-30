using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinNews.PostgresSQL.Models;

namespace XamarinNews.ModelsApi
{
    public class Response<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }

        public Response(string content)
        {
            JObject value = JObject.Parse(content);
            Status = value["status"].ToString() == "success";
            if (Status)
            {
                try
                {
                    Result = JsonConvert.DeserializeObject<T>(value["message"].ToString());
                }
                catch
                {
                    Message = value["message"].ToString();
                }
            }
            else
            {
                Message = value["message"].ToString();
            }
        }
    }
}
