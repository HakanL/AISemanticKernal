using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Collections;

namespace AISemanticKernal;

public class SemanticKernelTester
{
    [Test]
    public void ShouldCallOllamaUsingSemanticKernel()
    {
        var client = new AzureOpenAIClient(new Uri(Environment.GetEnvironmentVariable("AI_OpenAI_Url")),
            new ApiKeyCredential(Environment.GetEnvironmentVariable("AI_OpenAI_ApiKey")));
        var chatClient = client.GetChatClient("gpt-35-turbo");
        var result = chatClient.CompleteChat("What are your first two capabilities? Answer in JSON format");
        foreach (var part in result.Value.Content)
        {
            Console.Write(part.Text);
        }
    }

    [Test]
    public async Task AzureCodeTest()
    {

        // Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --prerelease

        async Task RunAsync()
        {
            // Retrieve the OpenAI endpoint from environment variables
            var endpoint = Environment.GetEnvironmentVariable("AI_OpenAI_Url");
            if (string.IsNullOrEmpty(endpoint))
            {
                Console.WriteLine("Please set the AI_OpenAI_Url environment variable.");
                return;
            }

            var key = Environment.GetEnvironmentVariable("AI_OpenAI_ApiKey");
            if (string.IsNullOrEmpty(key))
            {
                Console.WriteLine("Please set the AI_OpenAI_ApiKey environment variable.");
                return;
            }

            AzureKeyCredential credential = new AzureKeyCredential(key);

            // Initialize the AzureOpenAIClient
            AzureOpenAIClient azureClient = new(new Uri(endpoint), credential);

            // Initialize the ChatClient with the specified deployment name
            ChatClient chatClient = azureClient.GetChatClient("gpt-35-turbo");

            // Create a list of chat messages
            var messages = new List<ChatMessage>
          {
              new SystemChatMessage("You are an AI assistant that helps people find information."),
              new UserChatMessage("What are 3 things to visit in Seattle?")
          };


            // Create chat completion options
            var options = new ChatCompletionOptions
            {
                Temperature = (float)0.7,
                MaxOutputTokenCount = 800,

                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            try
            {
                // Create the chat completion request
                ChatCompletion completion = await chatClient.CompleteChatAsync(messages, options);

                // Print the response
                if (completion.Content != null && completion.Content.Count > 0)
                {
                    Console.WriteLine($"{completion.Content[0].Kind}: {completion.Content[0].Text}");
                }
                else
                {
                    Console.WriteLine("No response received.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        await RunAsync();
    }
}