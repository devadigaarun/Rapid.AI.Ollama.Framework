using System.Text.Json.Serialization;

namespace Rapid.AI.Ollama.Framework.Models
{
    internal class ChatRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("messages")]
        public List<ChatMessage> Messages { get; set; } = new();

        [JsonPropertyName("stream")]
        public bool Stream { get; set; } = false; // <-- Required to avoid JsonReaderException
    }
}
