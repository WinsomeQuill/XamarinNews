using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System;
using XamarinNews.MongoDB.Models;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading;
using RestSharp;
using static Android.Provider.ContactsContract;

namespace XamarinNews
{
    public static class Api
    {
        private static RestClient _client = new RestClient("http://192.168.2.104:27000");

        public async static Task<JObject> GetAvatar(string login)
        {
            RestRequest request = new RestRequest($"/get-profile-avatar?login={login}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            return value;
        }

        public async static Task<JObject> RegistrationUser(User user)
        {
            string json = JsonConvert.SerializeObject(user);

            RestRequest request = new RestRequest($"/insert-user", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(json);
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            return value;
        }

        public async static Task<JObject> LoginUser(string login, string pass)
        {
            RestRequest request = new RestRequest($"/login-user?login={login}&password={pass}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            return value;
        }

        public async static Task<JObject> SetAvatar(string login, byte[] crop_avatar, byte[] full_avatar)
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
            return value;
        }

        public async static Task<JObject> UserInfo(int id)
        {
            RestRequest request = new RestRequest($"/user-info?id={id}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await _client.ExecuteAsync(request);
            JObject value = JObject.Parse(response.Content);
            return value;
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

        public async static Task<JObject> FollowingUser(int author_id, int follower_id)
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
            return value;
        }

        public async static Task<JObject> RemoveFollowingUser(int author_id, int follower_id)
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
            return value;
        }
    }
}
