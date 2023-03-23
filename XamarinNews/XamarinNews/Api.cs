using System.Threading.Tasks;
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
using XamarinNews.ModelsApi;

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
            Response<string> result = new Response<string>(response.Content);
            return result.Status;
        }

        public async static Task<User> LoginUser(string login, string pass)
        {
            RestRequest request = new RestRequest($"/login-user?login={login}&password={pass}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            Response<User> result = new Response<User>(response.Content);
            if (result.Status)
            {
                return result.Message;
            }
            return null;
        }

        public async static Task<bool> SetAvatar(string login, byte[] crop_avatar, byte[] full_avatar)
        {
            object json = new
            {
                login,
                crop_avatar,
                full_avatar,
            };

            RestRequest request = new RestRequest("/set-profile-avatar", Method.Post);
            request.AddJsonBody(json);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            Response<string> result = new Response<string>(response.Content);
            return result.Status;
        }

        public async static Task<User> UserInfo(int id)
        {
            RestRequest request = new RestRequest($"/user-info?user_id={id}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            Response<User> result = new Response<User>(response.Content);
            if (result.Status)
            {
                return result.Message;
            }
            return null;
        }

        public async static Task<bool> IsUserFollowed(int author_id, int follow_id)
        {
            RestRequest request = new RestRequest($"/is-user-followed?author_user_id={author_id}&follower_user_id={follow_id}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            Response<string> result = new Response<string>(response.Content);
            return result.Status;
        }

        public async static Task<int> GetUserCountFollowers(int user_id)
        {
            RestRequest request = new RestRequest($"/user-count-followers?user_id={user_id}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            Response<int> result = new Response<int>(response.Content);
            return result.Message;
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
            Response<string> result = new Response<string>(response.Content);
            return result.Status;
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
            Response<string> result = new Response<string>(response.Content);
            return result.Status;
        }

        public async static Task<bool> InsertArticle(int author_id, byte[] image, string title, string description)
        {
            object json = new
            {
                author_id,
                image,
                title,
                description
            };

            RestRequest request = new RestRequest("/insert-article", Method.Post);
            request.AddJsonBody(json);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            Response<string> result = new Response<string>(response.Content);
            return result.Status;
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
            Response<string> result = new Response<string>(response.Content);
            return result.Status;
        }

        public async static Task<List<Article>> GetArticles()
        {
            RestRequest request = new RestRequest("/get-articles", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            Response<List<Article>> result = new Response<List<Article>>(response.Content);
            if (result.Status)
            {
                return result.Message;
            }
            return null;
        }

        public async static Task<List<Article>> GetArticlesFromUser(int user_id)
        {
            RestRequest request = new RestRequest($"/get-articles-from-user?user_id={user_id}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            Response<List<Article>> result = new Response<List<Article>>(response.Content);
            if (result.Status)
            {
                return result.Message;
            }
            return null;
        }

        public async static Task<bool> InsertArticleComment(int user_id, int article_id, string message)
        {
            object json = new
            {
                user_id,
                article_id,
                message,
            };

            RestRequest request = new RestRequest("/insert-article-comment", Method.Post);
            request.AddJsonBody(json);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            Response<string> result = new Response<string>(response.Content);
            return result.Status;
        }

        public async static Task<List<Comment>> GetArticleComments(int article_id)
        {
            RestRequest request = new RestRequest($"/get-article-comments?article_id={article_id}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            Response<List<Comment>> result = new Response<List<Comment>>(response.Content);
            if (result.Status)
            {
                return result.Message;
            }
            return null;
        }
    }
}
