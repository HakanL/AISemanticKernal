namespace AISemanticKernel;

public enum EnvironmentVariable
{
    AI_OpenAI_Model,
    AI_OpenAI_Url,
    AI_OpenAI_ApiKey,
    AI_Ollama_Url,
    AI_Ollama_Model
}

public static class EnvironmentVariableExtensions
{
    public static string Get(this EnvironmentVariable variable)
    {
        var value = Environment.GetEnvironmentVariable(variable.ToString());
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(variable.ToString());
        }
        return value;
    }
}