using Azure.AI.OpenAI;
using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.TextGeneration;
using System.ClientModel;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using OllamaSharp;

namespace AISemanticKernel;

public class LlmTesterBase
{
    protected readonly string OllamaEndpoint = EnvironmentVariable.AI_Ollama_Url.Get();
    protected readonly string OllamaModel = EnvironmentVariable.AI_Ollama_Model.Get();
    protected readonly ChatHistory ChatHistory = new ChatHistory();
    protected ServiceProvider ServiceProvider;

    protected readonly string AzureOpenIdEndpoint = EnvironmentVariable.AI_OpenAI_Url.Get();
    protected readonly string AzureOpenIdModel = EnvironmentVariable.AI_OpenAI_Model.Get();
    protected readonly string AzureOpenIdApiKey = EnvironmentVariable.AI_OpenAI_ApiKey.Get();
    
    public LlmTesterBase()
    {
        ServiceCollection ioc = new ServiceCollection();
        ioc.AddOllamaAIChatCompletion(OllamaModel, OllamaEndpoint, ServiceId.Ollama.ToString());
        ioc.AddAzureOpenAIChatCompletion(AzureOpenIdModel, AzureOpenIdEndpoint, AzureOpenIdApiKey, ServiceId.AzureOpenId.ToString());
        ioc.AddKernel();

        ServiceProvider = ioc.BuildServiceProvider();

        ChatHistory.AddSystemMessage("Only respond to the user with single word answers.");
    }
}

public static class IServicesCollectionExtensions
{
    public static IServiceCollection AddOllamaAIChatCompletion(
        this IServiceCollection services,
        string modelName,
        string endpoint,
        string? serviceId = null)
    {
        Func<IServiceProvider, object?, OllamaChatCompletionService> chatFactory = 
            (serviceProvider, _) => 
            new(modelName, endpoint, new HttpClient(), 
                serviceProvider.GetService<ILoggerFactory>());

        Func<IServiceProvider, object?, OllamaTextGenerationService> textFactory = 
            (serviceProvider, _) => 
            new(modelName, endpoint, new HttpClient(), 
                serviceProvider.GetService<ILoggerFactory>());

        services.AddKeyedSingleton<IChatCompletionService>(serviceId, chatFactory);
        services.AddKeyedSingleton<ITextGenerationService>(serviceId, textFactory);
        services.AddKeyedSingleton<OllamaChatCompletionService>(serviceId, chatFactory);
        services.AddKeyedSingleton<OllamaTextGenerationService>(serviceId, textFactory);


        return services;
    }
}

public enum ServiceId
{
    AzureOpenId,
    Ollama
}