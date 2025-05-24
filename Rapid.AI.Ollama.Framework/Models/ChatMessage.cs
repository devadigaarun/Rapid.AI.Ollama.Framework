using System.Text.Json.Serialization;

namespace Rapid.AI.Ollama.Framework.Models
{
    internal class ChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
}
