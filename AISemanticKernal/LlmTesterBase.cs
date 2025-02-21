using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.TextGeneration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace AISemanticKernel;

public class LlmTesterBase
{
    protected readonly string OllamaEndpoint = EnvironmentVariable.AI_Ollama_Url.Get();
    protected readonly string OllamaModel = EnvironmentVariable.AI_Ollama_Model.Get();
    protected readonly ChatHistory ChatHistory = new ChatHistory();
    protected ServiceProvider ServiceProvider => Ioc.BuildServiceProvider();

    protected readonly string AzureOpenIdEndpoint = EnvironmentVariable.AI_OpenAI_Url.Get();
    protected readonly string AzureOpenIdModel = EnvironmentVariable.AI_OpenAI_Model.Get();
    protected readonly string AzureOpenIdApiKey = EnvironmentVariable.AI_OpenAI_ApiKey.Get();
    protected readonly ServiceCollection Ioc = [];

    public LlmTesterBase()
    {
        Ioc.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));
        // ioc.AddOllamaAIChatCompletion(OllamaModel, OllamaEndpoint, ServiceId.Ollama.ToString());
#pragma warning disable SKEXP0070
        Ioc.AddOllamaChatCompletion(OllamaModel, new Uri(OllamaEndpoint), ServiceId.Ollama.ToString());
#pragma warning restore SKEXP0070
        Ioc.AddAzureOpenAIChatCompletion(AzureOpenIdModel, AzureOpenIdEndpoint, AzureOpenIdApiKey, ServiceId.AzureOpenId.ToString());
        Ioc.AddKernel();

        ChatHistory.AddSystemMessage("Only respond to the user with single word answers.");
    }
}

// ReSharper disable once InconsistentNaming
// public static class IServicesCollectionExtensions
// {
//     public static IServiceCollection AddOllamaAIChatCompletion(
//         this IServiceCollection services,
//         string modelName,
//         string endpoint,
//         string? serviceId = null)
//     {
//         Func<IServiceProvider, object?, OllamaChatCompletionService> chatFactory = 
//             (serviceProvider, _) => 
//             new(modelName, endpoint, new HttpClient(), 
//                 serviceProvider.GetService<ILoggerFactory>());
//
//         Func<IServiceProvider, object?, OllamaTextGenerationService> textFactory = 
//             (serviceProvider, _) => 
//             new(modelName, endpoint, new HttpClient(), 
//                 serviceProvider.GetService<ILoggerFactory>());
//
//         services.AddKeyedSingleton<IChatCompletionService>(serviceId, chatFactory);
//         services.AddKeyedSingleton<ITextGenerationService>(serviceId, textFactory);
//         services.AddKeyedSingleton<OllamaChatCompletionService>(serviceId, chatFactory);
//         services.AddKeyedSingleton<OllamaTextGenerationService>(serviceId, textFactory);
//
//
//         return services;
//     }
// }

public enum ServiceId
{
    AzureOpenId,
    Ollama
}