using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Shouldly;

namespace AISemanticKernel;

[TestFixture]
public class ChatHistoryScenarioWithAzureOpenAITester
{
    private readonly string _endpoint = EnvironmentVariable.AI_OpenAI_Url.Get();
    private readonly string _model = EnvironmentVariable.AI_OpenAI_Model.Get();
    private readonly string _apiKey = EnvironmentVariable.AI_OpenAI_ApiKey.Get();
    private readonly IChatCompletionService _chatService;
    private readonly ChatHistory _chatHistory = new ChatHistory();

    public ChatHistoryScenarioWithAzureOpenAITester()
    {
        _chatService = new AzureOpenAIChatCompletionService(
            _model, _endpoint, _apiKey);
        _chatHistory.AddSystemMessage("Only respond to the user with single word answers.");
    }

    [TestCase("Hello")]
    [TestCase("My name is Jeffrey")]
    [TestCase("I am 45 years old")]
    [TestCase("Liana is Jeffrey's wife. She is 1 year older")]
    [TestCase("How old is Jeffrey's wife?", "46", true)]
    public async Task ShouldRememberHistoryOfChat(string prompt, string? expected = null, bool shouldAssert = false)
    {
        _chatHistory.AddUserMessage(prompt);
        IReadOnlyList<ChatMessageContent> result = await _chatService
            .GetChatMessageContentsAsync(_chatHistory);

        if (shouldAssert)
        {
            string resultValue = result[0].Content!;
            Console.WriteLine(resultValue);
            resultValue.ShouldBe(expected);
        }
    }
}