using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AISemanticKernel;

[TestFixture]
public class ChatHistoryScenarioWithOllamaTester
{
    private readonly string _endpoint = EnvironmentVariable.AI_Ollama_Url.Get();
    private readonly string _model = EnvironmentVariable.AI_Ollama_Model.Get();
    private readonly IChatCompletionService _chatService;
    private readonly ChatHistory _chatHistory = new ChatHistory();

    public ChatHistoryScenarioWithOllamaTester()
    {
        _chatService = new OllamaChatCompletionService(
            _model, _endpoint, new HttpClient(), null); ;
        _chatHistory.AddSystemMessage("Only respond to the user with single word answers.");
    }

    [TestCase("Hello")]
    [TestCase("My name is Jeffrey")]
    [TestCase("I am 45 years old")]
    [TestCase("Liana is Jeffrey's wife. She is 1 year older")]
    [TestCase("How old is Jeffrey's wife?")]
    public async Task ShouldRememberHistoryOfChat(string prompt)
    {
        _chatHistory.AddUserMessage(prompt);
        IReadOnlyList<ChatMessageContent> result = await _chatService.GetChatMessageContentsAsync(_chatHistory);

        foreach (var resultItem in result)
        {
            Console.WriteLine(resultItem.Content);
        }
    }
}