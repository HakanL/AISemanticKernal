using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Shouldly;

namespace AISemanticKernel;

[TestFixture]
public class ChatHistoryScenarioWithOllamaTester : LlmTesterBase
{
    [TestCase("Hello")]
    [TestCase("My name is Jeffrey")]
    [TestCase("I am 45 years old")]
    [TestCase("Liana is Jeffrey's wife. She is 1 year older")]
    [TestCase("How old is Jeffrey's wife?", "46", true)]
    public async Task ShouldRememberHistoryOfChat(string prompt, string? expected = null, bool shouldAssert = false)
    {
        ChatHistory.AddUserMessage(prompt);
        IReadOnlyList<ChatMessageContent> result = await ServiceProvider
            .GetRequiredKeyedService<IChatCompletionService>(ServiceId.Ollama.ToString())
            .GetChatMessageContentsAsync(ChatHistory);

        if (shouldAssert)
        {
            string resultValue = result[0].Content!;
            Console.WriteLine(resultValue);
            resultValue.ShouldBe(expected);
        }
    }
}