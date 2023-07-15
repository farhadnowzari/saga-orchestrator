using System.Text.Json;

namespace SagaOrchestrator.Models;

public class SagaResumeModel
{
    public string MessageName { get; set; }
    public string ProcessInstanceId { get; set; }
    public SagaResumeModel(string messageName, string processId)
    {
        MessageName = messageName;
        ProcessInstanceId = processId;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}