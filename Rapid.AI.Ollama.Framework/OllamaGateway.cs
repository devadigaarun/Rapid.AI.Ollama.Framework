using Rapid.AI.Ollama.Framework.Models;
using System.Text;
using System.Text.Json;

namespace Rapid.AI.Ollama.Framework
{
    internal class OllamaGateway : IOllamaGateway
    {
        static List<ChatMessage> ChatHistory = [];
        private static readonly HttpClient myHttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        public string BaseUrl { get; }
        public string Model { get; private set; }

        public OllamaGateway(string baseUrl, string model)
        {
            BaseUrl = baseUrl;
            Model = model;
        }
        public string GenerateAsync(string prompt, string model = "")
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                model = Model;
            }

            var requestBody = new
            {
                model,
                prompt,
                stream = true
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/generate")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            using var response = myHttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;

            response.EnsureSuccessStatusCode();

            using var stream = response.Content.ReadAsStreamAsync().Result;
            using var reader = new StreamReader(stream);

            var fullResponse = new StringBuilder();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLineAsync().Result;
                if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

                try
                {
                    using var doc = JsonDocument.Parse(line);
                    var part = doc.RootElement.GetProperty("response").GetString();
                    fullResponse.Append(part);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing JSON: {line} - {ex.Message}");
                }
            }

            return fullResponse.ToString();
        }

        public string ChatAsync(string prompt, string model = "")
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                model = Model;
            }

            ChatHistory.Add(item: new ChatMessage { Role = "user", Content = prompt });

            var chatRequest = new ChatRequest
            {
                Model = model,
                Messages = ChatHistory
            };

            var json = JsonSerializer.Serialize(chatRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = myHttpClient.PostAsync($"{BaseUrl}/api/chat", content).Result;
                response.EnsureSuccessStatusCode();
                var responseJson = response.Content.ReadAsStringAsync().Result;

                using var doc = JsonDocument.Parse(responseJson);
                var result = doc.RootElement.GetProperty("message").GetProperty("content").GetString();

                if (string.IsNullOrWhiteSpace(result))
                {
                    Console.WriteLine("No response received.");
                    return string.Empty;
                }

                ChatHistory.Add(item: new ChatMessage { Role = "assistant", Content = result });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return string.Empty;
        }
        public void ClearChatHistory()
        {
            ChatHistory.Clear();
        }
    }
}
