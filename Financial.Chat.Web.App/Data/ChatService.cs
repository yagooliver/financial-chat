using Financial.Chat.Domain.Shared.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Financial.Chat.Web.App.Data
{
    public class ChatService
    {
        public async Task<List<UserDto>> GetUser(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                List<UserDto> users = new List<UserDto>();
                client.BaseAddress = new Uri("https://localhost:44367/");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await client.GetAsync("api/user");
                try
                {
                    var actionResult = JsonConvert.DeserializeObject<ApiOkReturn>(await response.Content.ReadAsStringAsync());
                    users = JsonConvert.DeserializeObject<List<UserDto>>(JsonConvert.SerializeObject(actionResult.data));
                }
                catch (Exception e)
                {

                }

                return users;
            }
        }
        public async Task<List<MessageDto>> GetUseMessages(string token, string email)
        {
            using (HttpClient client = new HttpClient())
            {
                List<MessageDto> messages = new List<MessageDto>();
                client.BaseAddress = new Uri("https://localhost:44367/");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await client.GetAsync("api/user/messages/" + email);
                try
                {
                    var actionResult = JsonConvert.DeserializeObject<ApiOkReturn>(await response.Content.ReadAsStringAsync());
                    messages = JsonConvert.DeserializeObject<List<MessageDto>>(JsonConvert.SerializeObject(actionResult.data));
                }
                catch (Exception e)
                {

                }

                return messages;
            }
        }
        public async Task SendMessage(string token, string sender, string consumer, string message)
        {
            using (HttpClient client = new HttpClient())
            {
                List<UserDto> users = new List<UserDto>();
                client.BaseAddress = new Uri("https://localhost:44367/");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(content: JsonConvert.SerializeObject(new { sender, consumer, message }), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                await client.PostAsync("api/user/send", content);
            }
        }
    }
}
