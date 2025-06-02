# Rapid.AI.Ollama.Framework

**Rapid.AI.Ollama.Framework** is a lightweight .NET library that allows developers to interact with locally or remotely running [Ollama](https://ollama.com) models. It supports both **stateless prompt generation** and **contextual multi-turn chat conversations** using the Ollama REST API.-
*Say goodbye to API limits and monthly bills. Develop and run a powerful local AI that browses, codes, and thinks.*
---
## ✨ Features

- 📦 Simple and easy-to-integrate C# API
- 🤖 **Ollama Integration**: Send prompts and receive completions from local or remote Ollama servers.
- 🧼 **Stateful & Stateless Chat**: Clearable chat histories to manage conversational memory.
- 🔧 Supports streaming output for prompt generation

🤖 **Ollama Gateway** (`IOllamaGateway`)
  - Lightweight prompt-based chat and content generation
  - Statefull and Stateless by default, with optional history reset
---
## 📦 Installation

Install from [NuGet.org](https://www.nuget.org/packages/Rapid.AI.Ollama.Framework):

```bash
dotnet add package Rapid.AI.Ollama.Framework
```
Or via Package Manager Console:

```powershell
Install-Package Rapid.AI.Ollama.Framework
```
---

## 🚀 Getting Started

### 📦 Prerequisites

- [.NET 6 or later](https://dotnet.microsoft.com/)
- [Ollama](https://ollama.com) running locally (default port `http://localhost:11434`)
- A downloaded model (e.g., `llama3`, `llama3.2:1b`, etc.)

### 🧱 Example Model Names
- llama3
- llama3.2:1b
- mistral

Any other model available through Ollama

Ensure the model is already pulled by running:
```bash
ollama run llama3.2:1b
```
---
## 🧪 Usage

### 🏭 1. Factory Class
```csharp
public interface IOllamaGatewayFactory
{
    IOllamaGateway Create(string ollamaUrl, string model);
}
```
### 🤖 2. IOllamaGateway
```csharp
public interface IOllamaGateway
{
    string ChatAsync(string prompt, string model = "");
    void ClearChatHistory();
    string GenerateAsync(string prompt, string model = "");
}
```
#### ✨ 1. Chat Usage example with Context (Multi-Turn)
```csharp

var factory = new OllamaGatewayFactory();
var ollama = factory.Create(ollamaUrl, myModel);

string response = ollama.ChatAsync("Tell me about India?");
Console.WriteLine(response);

string responseUpdated = ollama.ChatAsync("What are the best tourist places?");
Console.WriteLine(responseUpdated);

ollama.ClearChatHistory();
```
#### ✨ 1. Content generation Usage example
```csharp

var factory = new OllamaGatewayFactory();
var ollama = factory.Create(ollamaUrl, myModel);

string response = ollama.GenerateAsync("Tell me about India?");
Console.WriteLine(response);
```
#### 🔄 To Clear Chat History:
```csharp
OllamaClient.ClearChatHistory();
```
---
### 📃 License
MIT License – free to use, modify, and distribute.
📧 aruna.devadiga@gmail.com

### 🤝 Contributions

Feature requests and improvements are welcome. Please fork and PR your changes!
📧 aruna.devadiga@gmail.com

---
