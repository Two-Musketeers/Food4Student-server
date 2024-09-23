using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
        var paginationHeader = new
        {
            currentPage,
            itemsPerPage,
            totalItems,
            totalPages
        };

        var camelCaseFormatter = new JsonSerializerSettings();
        camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();

        response.Headers.Append("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }
}
