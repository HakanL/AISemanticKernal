using Microsoft.SemanticKernel;

namespace AISemanticKernel;

public class DemographicsInvocationFilter : IFunctionInvocationFilter
{
    public Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        if(context.Function.Name == "GetPersonAge")
            throw new NotImplementedException();

        return next(context); 
    }
}