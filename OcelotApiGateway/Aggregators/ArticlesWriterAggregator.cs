using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;
using System.Net.Http.Headers;
using OcelotApiGateway.Dtos;

namespace OcelotApiGateway.Aggregators
{
    public class ArticlesWriterAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var articles = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<ArticleDto>>();
            var writers = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<WriterDto>>();

            articles?.ForEach(article =>
            {
                article.Writer = writers?.FirstOrDefault(a => a.Id == article.WriterId);
            });

            var jsonString = JsonConvert.SerializeObject(articles, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

            var stringContent = new StringContent(jsonString)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
        }
    }
}
