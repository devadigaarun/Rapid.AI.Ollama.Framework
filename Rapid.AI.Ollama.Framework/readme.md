# Rapid.AI.Ollama.Framework

`Rapid.AI.Ollama.Framework` is a lightweight C# client library that allows developers to interact with locally running [Ollama](https://ollama.com) models. It supports both **stateless prompt generation** and **contextual multi-turn chat conversations** using the Ollama REST API.

## ✨ Features

- 🔁 Stateless prompt generation using `/api/generate`
- 💬 Context-aware chat with conversation history using `/api/chat`
- 📦 Simple and easy-to-integrate C# API
- 🔧 Supports streaming output for prompt generation

---

## 🚀 Getting Started

### 📦 Prerequisites

- [.NET 6 or later](https://dotnet.microsoft.com/)
- [Ollama](https://ollama.com) running locally (default port `http://localhost:11434`)
- A downloaded model (e.g., `llama3`, `llama3.2:1b`, etc.)

---

## 🧪 Usage

### ✨ 1. Generate Prompt Response

```csharp
using Rapid.AI.Ollama.Framework;

string result = OllamaClient.Generate("http://localhost:11434/api/generate", "What is quantum physics?", "llama3.2:1b");

Console.WriteLine(result);

```

This uses the /api/generate endpoint with streaming enabled, and returns a stateless response for the given prompt.

### 💬 2. Chat with Context (Multi-Turn)
```csharp
using Rapid.AI.Ollama.Framework;

// First user message
string reply1 = OllamaClient.Chat("http://localhost:11434/api/chat", "Who was Marie Curie?", "llama3.2:1b");
Console.WriteLine("AI: " + reply1);

// Follow-up message
string reply2 = OllamaClient.Chat("http://localhost:11434/api/chat", "What was her contribution to science?", "llama3.2:1b");
Console.WriteLine("AI: " + reply2);
```
You can maintain context by using Chat(). The chat history is kept internally.

### 🔄 To Clear Chat History:
```csharp
OllamaClient.ClearChatHistory();
```
### 📌 Notes
Generate method uses the /api/generate endpoint and streams the output.

Chat method uses the /api/chat endpoint and maintains internal chat history.

Timeout is set to 5 minutes for long-running responses.

### 📁 Project Structure
```csharp
OllamaClient
├── Generate(...)        // Stateless streaming prompt generation
├── Chat(...)            // Stateful chat with message history
├── ClearChatHistory()   // Clears the internal chat history
```

### 🧱 Example Model Names
- llama3

- llama3.2:1b

- mistral

Any other model available through Ollama

Ensure the model is already pulled by running:
```bash
ollama run llama3.2:1b
```

### 📃 License
MIT License – free to use, modify, and distribute.

### 🤝 Contributions
Feature requests and improvements are welcome. Please fork and PR your changes!
