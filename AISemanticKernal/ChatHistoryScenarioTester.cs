using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AISemanticKernel;

public class ChatHistoryScenarioTester
{
    private readonly string _endpoint = EnvironmentVariable.AI_Ollama_Url.Get();
    private readonly string _model = EnvironmentVariable.AI_Ollama_Model.Get();
    private readonly IChatCompletionService _chatService;

    public ChatHistoryScenarioTester(IChatCompletionService chatService)
    {
        _chatService = new OllamaChatCompletionService(
            _model, _endpoint, new HttpClient(), null); ;
    }

    [TestCase("My name is Jeffrey")]
    [TestCase("I am 45 years old")]
    [TestCase("I race motorcycles")]
    public void SemanticKernelToOllama(string prompt)
    {
        var result = _chatService.GetChatMessageContentAsync(
            "what color is the sky during a sunny day? One word answer with no punctuation");
        Console.WriteLine(result.Result);
    }

}