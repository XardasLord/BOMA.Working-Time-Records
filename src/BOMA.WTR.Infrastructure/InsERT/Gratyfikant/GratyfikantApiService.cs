using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using BOMA.WTR.Application.InsERT;
using BOMA.WTR.Application.InsERT.Gratyfikant;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;

namespace BOMA.WTR.Infrastructure.InsERT.Gratyfikant;

public class GratyfikantApiService(IHttpClientFactory httpClientFactory) : IGratyfikantService
{
    private static readonly JsonSerializerOptions JsonSerializerDefaultOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<List<string>> SendHours(IEnumerable<EmployeeWorkingTimeRecordViewModel> records)
    {
        var payloadContent = PrepareContentRequest(records);
        
        var httpClient = PrepareHttpClient();
        using var response = await httpClient.PostAsync("api/gratyfikant", payloadContent);
        
        response.EnsureSuccessStatusCode();
        
         var responseModel = await response.Content.ReadFromJsonAsync<List<string>>();
         
         return responseModel;
    }

    private static StringContent PrepareContentRequest(object payload)
    {
        var payloadJson = JsonSerializer.Serialize(payload, JsonSerializerDefaultOptions);
        var payloadContent = new StringContent(payloadJson, Encoding.UTF8, MediaTypeNames.Application.Json);
        
        return payloadContent;
    }

    private HttpClient PrepareHttpClient() 
        => httpClientFactory.CreateClient(ExternalHttpClientNames.GratyfikantHttpClientName);
}