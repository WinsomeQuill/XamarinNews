using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinNews.PostgresSQL.Models;

namespace XamarinNews.ModelsApi
{
    public class Response<T>
    {
        public bool Status { get; set; }
        public T Message { get; set; }

        public Response(string content)
        {
            JObject value = JObject.Parse(content);
            Status = value["status"].ToString() == "success";
            if (Status)
            {
                try
                {
                    Message = JsonConvert.DeserializeObject<T>(value["message"].ToString());
                }
                catch
                {
                    Message = value["message"].Value<T>();
                }
            }
            else
            {
                Message = value["message"].Value<T>();
            }
        }
    }
}
