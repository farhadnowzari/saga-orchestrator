using System.Text;
using Microsoft.Extensions.Options;
using SagaOrchestrator.Models;
using SagaOrchestrator.Options;

namespace SagaOrchestrator.Contexts;

public class CamundaContext
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationOptions _applicationOptions;

    public CamundaContext(HttpClient httpClient, IOptions<ApplicationOptions> applicationOptions)
    {
        _httpClient = httpClient;
        _applicationOptions = applicationOptions.Value;
    }

    public void ResumeSaga(string processId)
    {
        var messagePostRequest = new HttpRequestMessage(HttpMethod.Post, $"{_applicationOptions.CamundaUrl}/message");
        var jsonContent = new StringContent(
            new SagaResumeModel("saga/resume", processId).ToString(),
            Encoding.UTF8,
            "application/json"
        );
        messagePostRequest.Content = jsonContent;
        var response = _httpClient.Send(messagePostRequest);
        response.EnsureSuccessStatusCode();
    }
}