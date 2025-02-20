using System.Runtime.InteropServices;
using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AISemanticKernel;

[TestFixture]
public class ToolInjectionScenarioTester : LlmTesterBase
{
    [Test]
    public async Task ShouldRetrieveFromIoc()
    {
        ChatHistory.AddUserMessage("what is 2+2?");
        IChatCompletionService chatService = ServiceProvider.GetRequiredKeyedService<IChatCompletionService>(ServiceId.Ollama.ToString());
        IReadOnlyList<ChatMessageContent> result = await chatService.GetChatMessageContentsAsync(ChatHistory);
        foreach (ChatMessageContent content2 in result)
        {
            Console.WriteLine(content2.Content);
        }
        
    }

    [Test]
    public async Task ShouldCallKernelToolUsingAzureOpenAI()
    {
        ServiceProvider provider = ServiceProvider;
        var chatService = provider.GetRequiredKeyedService<IChatCompletionService>(ServiceId.AzureOpenId.ToString());
        Kernel kernel = provider.GetRequiredService<Kernel>();
        kernel.ImportPluginFromType<Demographics>();

        IReadOnlyList<ChatMessageContent> result = await chatService.GetChatMessageContentsAsync("How old is Grandpa?", kernel:kernel, 
            executionSettings:new PromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            });
        foreach (ChatMessageContent content2 in result)
        {
            Console.WriteLine(content2.Content);
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
            "How old is Grandpa?", kernel: kernel,
            executionSettings: new PromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            });
        foreach (ChatMessageContent content2 in result)
        {
            Console.WriteLine(content2.Content);
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
                "Grandpa" => 80,
                _ => 0
            };
        }
    }


}