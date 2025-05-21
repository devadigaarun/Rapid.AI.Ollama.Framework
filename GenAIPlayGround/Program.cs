using Rapid.AI.Ollama.Framework;
using System.Diagnostics;
using System.Text;
class OllamaAgent
{
    static async Task Main()
    {
        // string apiUrl = "http://md2pyjzc.ad005.onehc.net:11434/api/generate";
        // string model = "codegemma:2b"; // "codegemma:7b"; // "llama3.3:70B"; // "llama3.2:1b"; // "llama3.3:70B"; // "gemma:7b";// "codegemma:7b"; // "llama3:latest"; // "llama3.2:1b"; // "codegemma:7b"; // "llama3:latest"; // "llama3.2:1b"; // "codegemma:7b"; // "llama3.2:1b"; // "codegemma:7b";// "llama3:latest"; // "llama3.2:1b"; // "llama3:latest";

        string ChatUrl = "http://localhost:11434/api/chat";
        string GenerateUrl = "http://localhost:11434/api/chat";
        string myModel = "llama3.2:1b";
        string myUrl = ChatUrl;

        while (true)
        {
            Console.Write("\nAsk Anything: ");
            var inputBuilder = new StringBuilder();

            bool isExitRequested = false;

            while (true)
            {
                var line = Console.ReadLine();                 
                if(line.ToLower().StartsWith("model:"))
                {
                    myModel = line.Substring(6).Trim();
                    Console.WriteLine($"Model changed to: {myModel}");
                    continue;
                }

                if (line.ToLower().Equals("mode:chat"))
                {
                    myUrl = ChatUrl;
                    Console.WriteLine($"Request Mode changed to: Chat");
                    continue;
                }
                else
                if (line.ToLower().Equals("mode:generate"))
                {
                    myUrl = GenerateUrl;
                    Console.WriteLine($"Request Mode changed to: Generate");
                    continue;
                }
                else
                if (line.ToLower().Equals("history:clear"))
                {
                    OllamaClient.ClearChatHistory();
                    Console.WriteLine($"Chat History cleared");
                    continue;
                }

                if (line.ToLower().Equals("exit"))
                {
                    isExitRequested = true;
                    break;
                }
                else
                if(line.ToLower().Equals("end"))
                {
                    break;
                }

                inputBuilder.AppendLine(line);
            }

            var prompt = inputBuilder.ToString();
            if (isExitRequested) break;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var response = OllamaClient.Chat(myUrl, prompt, myModel);// "codegemma:2b"); // "llama3.2:1b");

            stopwatch.Stop();
            TimeSpan responseTime = stopwatch.Elapsed;
            Console.WriteLine($"\n{response}\n");
        }
    }
}