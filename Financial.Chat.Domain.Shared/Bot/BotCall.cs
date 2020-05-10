using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Financial.Chat.Domain.Shared.Bot
{
    public class BotCall
    {
        private const string PREFIX = "https://stooq.com/q/l/?s=";
        private const string URL = ".us&f=sd2t2ohlcv&h&e=csv";

        public string CallServiceStock(string keyWord)
        {
            var url = $"{PREFIX}{keyWord}{URL}";
            var quote = GetStockInformation(url).Split(',')[13];
            var response = $"{keyWord} quote is ${quote} per share";
            return response;
        }

        private string GetStockInformation(string URI)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(PREFIX);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage result = Task.Run(async () => await client.GetAsync(URI)).Result;
                var objeto = Task.Run(async () => await result.Content.ReadAsStringAsync()).Result;
                
                return objeto;
            }
        }
        public bool IsStockCall(string receivedMessage) => string.Compare(receivedMessage, 0, "/stock=", 0, 7) == 0;
    }
}
