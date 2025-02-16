using OllamaSharp;
using OllamaSharp.Models.Chat;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using ChatRole = OllamaSharp.Models.Chat.ChatRole;

namespace AISemanticKernel
{
    public class OllamaApiClientTester
    {
        [Test]
        public void ShouldCallLocalLLM()
        {
            var ollamaApiClient = new OllamaApiClient("http://localhost:11434");
            var chatRequest = new ChatRequest
            {
                Model = "llama3.2",
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
    }
}
