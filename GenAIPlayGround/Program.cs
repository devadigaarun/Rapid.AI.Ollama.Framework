using Rapid.AI.Ollama.Framework;
using System.Diagnostics;
using System.Text;
class OllamaAgent
{
    static async Task Main()
    {
        // string apiUrl = "http://md2pyjzc.ad005.onehc.net:11434/api/generate";
        // string model = "codegemma:2b"; "llama3"; // "codegemma:7b"; // "llama3.3:70B"; // "llama3.2:1b"; // "llama3.3:70B"; // "gemma:7b";// "codegemma:7b"; // "llama3:latest"; // "llama3.2:1b"; // "codegemma:7b"; // "llama3:latest"; // "llama3.2:1b"; // "codegemma:7b"; // "llama3.2:1b"; // "codegemma:7b";// "llama3:latest"; // "llama3.2:1b"; // "llama3:latest";

        string ollamaUrl = "http://localhost:11434";
        string myModel = "llama3.2:1b";

        var factory = new OllamaGatewayFactory();
        var ollamaGateway = factory.Create(ollamaUrl, myModel);

        bool isChatRequsted = false;

        while (true)
        {
            if (isChatRequsted)
            {
                Console.Write("\nAsk Anything<chat> Type . to complete: ");
            }
            else
            {
                Console.Write("\nAsk Anything<generate> Type . to complete: ");
            }

            var isSettingsChanged = false;
            bool isExitRequested = false;
            var inputBuilder = new StringBuilder();

            while (true)
            {
                var line = Console.ReadLine();

                if (line.ToLower().StartsWith("model:"))
                {
                    myModel = line.Substring(6).Trim();
                    Console.WriteLine($"Model changed to: {myModel}");
                    isSettingsChanged = true;
                    break;
                }
                else
                if (line.ToLower().Equals("mode:chat"))
                {
                    isChatRequsted = true;
                    Console.WriteLine($"Request Mode changed to: ChatAsync");
                    isSettingsChanged = true;
                    break;
                }
                else
                if (line.ToLower().Equals("mode:generate"))
                {
                    isChatRequsted = false;
                    Console.WriteLine($"Request Mode changed to: GenerateAsync");
                    isSettingsChanged = true;
                    break;
                }
                else
                if (line.ToLower().Equals("history:clear"))
                {
                    ollamaGateway.ClearChatHistory();
                    Console.WriteLine($"ChatAsync History cleared");
                    isSettingsChanged = true;

                    break;
                }

                if (line.ToLower().Equals("exit"))
                {
                    isExitRequested = true;
                    break;
                }
                else
                if (line.ToLower().Equals("end"))
                {
                    break;
                }
                else
                if (line.ToLower().Equals("."))
                {
                    break;
                }

                inputBuilder.AppendLine(line);
            }

            if (isSettingsChanged)
            {
                continue;
            }
            if (isExitRequested)
            {
                Console.WriteLine("I am exiting... bye!!");
                break;
            }

            var prompt = inputBuilder.ToString();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string response = string.Empty;

            if (isChatRequsted)
            {
                response = ollamaGateway.ChatAsync(prompt, myModel);
            }
            else
            {
                response = ollamaGateway.GenerateAsync(prompt, myModel);
            }

            stopwatch.Stop();
            TimeSpan responseTime = stopwatch.Elapsed;
            Console.WriteLine($"\n{response}\n");
        }
    }
}