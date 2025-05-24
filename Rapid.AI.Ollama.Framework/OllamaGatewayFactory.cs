namespace Rapid.AI.Ollama.Framework
{
    public class OllamaGatewayFactory : IOllamaGatewayFactory
    {
        public IOllamaGateway Create(string ollamaUrl, string model)
        {
            return new OllamaGateway(ollamaUrl, model);
        }
    }
}
