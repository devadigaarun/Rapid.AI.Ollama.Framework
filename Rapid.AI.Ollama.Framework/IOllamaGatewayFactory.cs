namespace Rapid.AI.Ollama.Framework
{
    public interface IOllamaGatewayFactory
    {
        IOllamaGateway Create(string ollamaUrl, string model);
    }
}
