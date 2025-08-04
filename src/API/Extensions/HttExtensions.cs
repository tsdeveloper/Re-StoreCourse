using API.RequestsHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API.Extensions;

public static class HttExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, MetaData metada)
    {
        response.Headers.Add("Pagination", JsonConvert.SerializeObject(metada, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        }));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

    }
}