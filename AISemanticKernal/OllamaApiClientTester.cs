using OllamaSharp;
using OllamaSharp.Models.Chat;
using System.Threading.Tasks;
using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.ChatCompletion;
using ChatRole = OllamaSharp.Models.Chat.ChatRole;

namespace AISemanticKernel
{
    public class OllamaApiClientTester
    {
        private string _endpoint = "http://localhost:11434";
        private string _model = "llama3.2";

        [Test]
        public void SemanticKernelToOllama()
        {
            IChatCompletionService chatService = new OllamaChatCompletionService(
                _model, _endpoint, new HttpClient(), null);

            var result = chatService.GetChatMessageContentAsync(
                "what color is the sky during a sunny day? One word answer with no punctuation");
            Console.WriteLine(result.Result);
        }

        [Test]
        public void ShouldCallLocalLLM()
        {
            var ollamaApiClient = new OllamaApiClient(_endpoint);
            var chatRequest = new ChatRequest
            {
                Model = _model,
                Messages =
                [
                    new Message(){Content = "What are your capabilities? Only list 3? use json response only", Role = ChatRole.User}
                ]
            };


            var chatResponse = ollamaApiClient.ChatAsync(chatRequest);

            foreach (var message in chatResponse.ToBlockingEnumerable())
            {
                Console.Write(message.Message.Content);
            }
        }

        [Test, Repeat(5), CancelAfter(20000)]
        public void SemanticKernelToOllamaMultiTest()
        {
            IChatCompletionService chatService = new OllamaChatCompletionService(
                _model, _endpoint, new HttpClient(), null);

            var result = chatService.GetChatMessageContentAsync(
                "what color is the sky during a sunny day? One word answer with no punctuation");
            Console.WriteLine(result.Result);
        }
    }
}
