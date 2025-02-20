using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AISemanticKernel;

[TestFixture]
public class ChatHistoryScenarioWithOllamaTester : LlmTesterBase
{
    [TestCase("Hello")]
    [TestCase("My name is Jeffrey")]
    [TestCase("I am 45 years old")]
    [TestCase("Liana is Jeffrey's wife. She is 1 year older")]
    [TestCase("How old is Jeffrey's wife?")]
    public async Task ShouldRememberHistoryOfChat(string prompt)
    {
        ChatHistory.AddUserMessage(prompt);
        IReadOnlyList<ChatMessageContent> result = await ServiceProvider.GetRequiredKeyedService<IChatCompletionService>(ServiceId.Ollama.ToString())
            .GetChatMessageContentsAsync(ChatHistory);

        foreach (var resultItem in result)
        {
            Console.WriteLine(resultItem.Content);
        }
    }
}