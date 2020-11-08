using Financial.Chat.Domain.Shared.Entity;
using Financial.Chat.Web.App.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Financial.Chat.Web.App.Data
{
    public class ChatService
    {
        private const string URL = "http://financialchat/";

        public string GetURL() => URL;
        public async Task<List<UserDto>> GetUser(string token)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (HttpClient client = new HttpClient(clientHandler))
            {
                List<UserDto> users = new List<UserDto>();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await client.GetAsync("api/user");
                try
                {
                    var actionResult = JsonConvert.DeserializeObject<ApiOkReturn>(await response.Content.ReadAsStringAsync());
                    users = JsonConvert.DeserializeObject<List<UserDto>>(JsonConvert.SerializeObject(actionResult.data));
                }
                catch
                {

                }

                return users;
            }
        }

        public async Task<HttpResponseMessage> PostNewUser(NewUserViewModel model)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (HttpClient client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                var content = new StringContent(content: JsonConvert.SerializeObject(model), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
                var response = await client.PostAsync("api/user/signin", content);

                return response;
            }
        }

        public async Task<HttpResponseMessage> Login(string email, string password)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (HttpClient client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                var content = new StringContent(content: JsonConvert.SerializeObject(new { email, password }), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
                var response = await client.PostAsync("api/login", content);

                return response;
            }
        }

        public async Task<List<MessageDto>> GetUseMessages(string token, string email)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (HttpClient client = new HttpClient(clientHandler))
            {
                List<MessageDto> messages = new List<MessageDto>();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await client.GetAsync("api/user/messages/" + email);
                try
                {
                    var actionResult = JsonConvert.DeserializeObject<ApiOkReturn>(await response.Content.ReadAsStringAsync());
                    messages = JsonConvert.DeserializeObject<List<MessageDto>>(JsonConvert.SerializeObject(actionResult.data));
                }
                catch 
                {

                }

                return messages;
            }
        }
        public async Task SendMessage(string token, string sender, string consumer, string message)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (HttpClient client = new HttpClient(clientHandler))
            {
                List<UserDto> users = new List<UserDto>();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(content: JsonConvert.SerializeObject(new { sender, consumer, message }), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                await client.PostAsync("api/user/send", content);
            }
        }
    }
}
