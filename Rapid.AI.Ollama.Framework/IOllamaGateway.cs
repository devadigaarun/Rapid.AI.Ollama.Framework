namespace Rapid.AI.Ollama.Framework
{
    public interface IOllamaGateway
    {
        string ChatAsync(string prompt, string model = "");
        void ClearChatHistory();
        string GenerateAsync(string prompt, string model = "");
    }
}