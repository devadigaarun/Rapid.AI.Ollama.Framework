using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rapid.AI.Ollama.Framework
{
    public static class OllamaClient
    {
        static List<Message> ChatHistory = new List<Message>();
        public static string Generate(string url, string prompt, string model)
        {
            var requestBody = new
            {
                model = model,
                prompt = prompt,
                stream = true
            };

            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            using var response = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;

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
        
        public static string Chat(string url, string prompt, string model)
        {
            ChatHistory.Add(item: new Message { Role = "user", Content = prompt });

            var chatRequest = new ChatRequest
            {
                Model = model,
                Messages = ChatHistory
            };

            var json = JsonSerializer.Serialize(chatRequest);
            using var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = client.PostAsync(url, content).Result;
                response.EnsureSuccessStatusCode();
                var responseJson = response.Content.ReadAsStringAsync().Result;

                using var doc = JsonDocument.Parse(responseJson);
                var result = doc.RootElement.GetProperty("message").GetProperty("content").GetString();

                if(string.IsNullOrWhiteSpace(result))
                {
                    Console.WriteLine("No response received.");
                    return string.Empty;
                }

                ChatHistory.Add(item: new Message { Role = "user", Content = result });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return string.Empty;
        }
        public static void ClearChatHistory()
        {
            ChatHistory.Clear();
        }

        class Message
        {
            [JsonPropertyName("role")]
            public string Role { get; set; } = string.Empty;

            [JsonPropertyName("content")]
            public string Content { get; set; } = string.Empty;
        }

        class ChatRequest
        {
            [JsonPropertyName("model")]
            public string Model { get; set; } = string.Empty;

            [JsonPropertyName("messages")]
            public List<Message> Messages { get; set; } = new();

            [JsonPropertyName("stream")]
            public bool Stream { get; set; } = false; // <-- Required to avoid JsonReaderException
        }
    }
}
