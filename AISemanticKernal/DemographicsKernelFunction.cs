using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace AISemanticKernel;

public class DemographicsKernelFunction
{
    [KernelFunction]
    [System.ComponentModel.Description("Gets the age of a person.")]
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