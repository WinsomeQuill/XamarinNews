﻿using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System;
using XamarinNews.PostgresSQL.Models;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading;
using RestSharp;
using System.Collections.Generic;

namespace XamarinNews
{
    public static class Api
    {
        private static RestClient _client = new RestClient("http://192.168.2.104:27000");

        public async static Task<bool> RegistrationUser(RegisterUser user)
        {
            string json = JsonConvert.SerializeObject(user);

            RestRequest request = new RestRequest($"/insert-user", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(json);
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            if (value["status"].ToString() == "success")
            {
                return true;
            }
            return false;
        }

        public async static Task<User> LoginUser(string login, string pass)
        {
            RestRequest request = new RestRequest($"/login-user?login={login}&password={pass}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            if (value["status"].ToString() == "success")
            {
                User user = JsonConvert.DeserializeObject<User>(value["message"].ToString());
                return user;
            }
            return null;
        }

        public async static Task<bool> SetAvatar(string login, byte[] crop_avatar, byte[] full_avatar)
        {
            string crop_content = Convert.ToBase64String(crop_avatar);
            string full_content = Convert.ToBase64String(full_avatar);

            object json = new
            {
                login = login,
                crop_avatar = crop_content,
                full_avatar = full_content,
            };

            RestRequest request = new RestRequest("/set-profile-avatar", Method.Post);
            request.AddJsonBody(json);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            if (value["status"].ToString() == "success")
            {
                return true;
            }
            return false;
        }

        public async static Task<User> UserInfo(int id)
        {
            RestRequest request = new RestRequest($"/user-info?id={id}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            if (value["status"].ToString() == "success")
            {
                User user = JsonConvert.DeserializeObject<User>(value["message"].ToString());
                return user;
            }
            return null;
        }

        public async static Task<bool> IsUserFollowed(int author_id, int follow_id)
        {
            RestRequest request = new RestRequest($"/is-user-followed?author_user_id={author_id}&follower_user_id={follow_id}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            return (bool)value["message"];
        }

        public async static Task<int> GetUserCountFollowers(int user_id)
        {
            RestRequest request = new RestRequest($"/user-count-followers?id={user_id}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            return (int)value["message"];
        }

        public async static Task<bool> FollowingUser(int author_id, int follower_id)
        {
            object json = new
            {
                author_id,
                follower_id,
            };

            RestRequest request = new RestRequest("/following-user", Method.Post);
            request.AddJsonBody(json);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            if (value["status"].ToString() == "success")
            {
                return true;
            }
            return false;
        }

        public async static Task<bool> RemoveFollowingUser(int author_id, int follower_id)
        {
            object json = new
            {
                author_id,
                follower_id,
            };

            RestRequest request = new RestRequest("/remove-following-user", Method.Post);
            request.AddJsonBody(json);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            if (value["status"].ToString() == "success")
            {
                return true;
            }
            return false;
        }

        public async static Task<bool> InsertArticle(int author_id, int follower_id)
        {
            object json = new
            {
                author_id,
                follower_id,
            };

            RestRequest request = new RestRequest("/insert-article", Method.Post);
            request.AddJsonBody(json);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            if (value["status"].ToString() == "success")
            {
                return true;
            }
            return false;
        }

        public async static Task<bool> RemoveArticle(int article_id, int user_id)
        {
            object json = new
            {
                article_id,
                user_id,
            };

            RestRequest request = new RestRequest("/remove-article", Method.Post);
            request.AddJsonBody(json);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            if (value["status"].ToString() == "success")
            {
                return true;
            }
            return false;
        }

        public async static Task<List<Article>> GetArticles()
        {
            RestRequest request = new RestRequest("/get-articles", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            if (value["status"].ToString() == "success")
            {
                List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(value["message"].ToString());
                return articles;
            }
            return null;
        }

        public async static Task<List<Article>> GetArticlesFromUser(int user_id)
        {
            RestRequest request = new RestRequest($"/get-articles-from-user?user_id={user_id}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            if (value["status"].ToString() == "success")
            {
                List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(value["message"].ToString());
                return articles;
            }
            return null;
        }
    }
}
