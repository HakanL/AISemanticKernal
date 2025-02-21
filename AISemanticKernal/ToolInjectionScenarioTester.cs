using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Shouldly;

namespace AISemanticKernel;

[TestFixture]
public class ToolInjectionScenarioTester : LlmTesterBase
{
    [Test]
    public async Task ShouldRetrieveFromIoc()
    {
        ChatHistory.AddUserMessage("what is 2+2? Respond with the digit and no punctuation");
        IChatCompletionService chatService = ServiceProvider.GetRequiredKeyedService<IChatCompletionService>(ServiceId.Ollama.ToString());
        IReadOnlyList<ChatMessageContent> result = await chatService.GetChatMessageContentsAsync(ChatHistory);
        foreach (ChatMessageContent content2 in result)
        {
            Console.WriteLine(content2.Content);
            content2.Content.ShouldBe("4");
        }
        
    }

    [Test]
    public async Task ShouldCallKernelToolUsingAzureOpenAI()
    {
        ServiceProvider provider = ServiceProvider;
        var chatService = provider.GetRequiredKeyedService<IChatCompletionService>(ServiceId.AzureOpenId.ToString());
        Kernel kernel = provider.GetRequiredService<Kernel>();
        kernel.ImportPluginFromType<Demographics>();

        IReadOnlyList<ChatMessageContent> result = await chatService.GetChatMessageContentsAsync(
            "How old is Grandpa? Respond with single value with no punctuation", kernel:kernel, 
            executionSettings:new PromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            });
        foreach (ChatMessageContent content2 in result)
        {
            Console.WriteLine(content2.Content);
            content2.Content.ShouldBe("81");
        }

        
    }

    [Test]
    public async Task ShouldCallKernelToolUsingOllama()
    {
        ServiceProvider provider = ServiceProvider;
        var chatService = provider.GetRequiredKeyedService<IChatCompletionService>(
            ServiceId.Ollama.ToString());
        Kernel kernel = provider.GetRequiredService<Kernel>();
        kernel.ImportPluginFromType<Demographics>();

        IReadOnlyList<ChatMessageContent> result = await chatService.GetChatMessageContentsAsync(
            "How old is Grandpa? Respond with single value with no punctuation", kernel: kernel,
            executionSettings: new PromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            });
        foreach (ChatMessageContent content2 in result)
        {
            Console.WriteLine(content2.Content);
            content2.Content.ShouldBe("81");
        }


    }

    [Test]
    public async Task ShouldCallKernelToolTwiceUsingOllama()
    {
        ServiceProvider provider = ServiceProvider;
        var chatService = provider.GetRequiredKeyedService<IChatCompletionService>(
            ServiceId.Ollama.ToString());
        Kernel kernel = provider.GetRequiredService<Kernel>();
        kernel.ImportPluginFromType<Demographics>();

        IReadOnlyList<ChatMessageContent> result = await chatService.GetChatMessageContentsAsync(
            "Is Grandpa's age older than Liana's age? Single word response, no punctuation", kernel: kernel,
            executionSettings: new PromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            });
        foreach (ChatMessageContent content2 in result)
        {
            Console.WriteLine(content2.Content);
            content2.Content.ToLower().ShouldBe("yes");
        }


    }

    public class Demographics    
    {
        [KernelFunction]
        public int GetPersonAge(string name)
        {
            return name switch
            {
                "Jeffrey" => 45,
                "Liana" => 46,
                "Grandpa" => 81,
                _ => 0
            };
        }
    }


}