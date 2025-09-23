using System.Collections.Concurrent;
using AspNetSamples.DataAccess.Commands;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace AspNetSamples.Services
{
    public class ArticleRateService : IArticleRateService
    {
        #region http_params

        class Message
        {
            public string role { get; set; }
            public string content { get; set; }
        }

        class Request
        {
            public string model { get; set; }
            public bool stream { get; set; }
            public List<Message> messages { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        class Response
        {
            public string model { get; set; }
            public DateTime created_at { get; set; }
            public Message message { get; set; }
            public bool done { get; set; }
            public long total_duration { get; set; }
            public long load_duration { get; set; }
            public long prompt_eval_count { get; set; }
            public long prompt_eval_duration { get; set; }
            public long eval_count { get; set; }
            public long eval_duration { get; set; }
        }


        #endregion

        private readonly ILogger<ArticleRateService> _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public ArticleRateService(ILogger<ArticleRateService> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task RateArticlesAsync(CancellationToken token = default)
        {
            var apiUrl = _configuration["AppSettings:OllamaApiUrl"];
            // Call the API using _mediator and return the result

            var articlesToRate = await _mediator.Send(new GetArticlesToRateQuery(), token);

            //try to parallel the requests to the llm api if you have powerful hardware
            foreach (var articleDto in articlesToRate)
            {
                var articleText =
                    await _mediator.Send(new GetArticleTextByIdQuery { ArticleId = articleDto.Id }, token);

                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                    request.Headers.Add("Accept", "application/json");


                    request.Content = JsonContent.Create(new Request()
                    {
                        model = "gpt-oss:20b",
                        messages = new List<Message>
                        {
                            new Message
                            {
                                role = "user",
                                content =
                                    $"Rate this article by positivity rate from -10 to 10 and provide only value as response. {Environment.NewLine} {articleText}"
                            }
                        },
                        stream = false
                    });

                    var response = await httpClient.SendAsync(request, token);
                    if (response.IsSuccessStatusCode)
                    {
                        var resultString = await response.Content.ReadAsStringAsync(token);
                        var responseData = JsonConvert.DeserializeObject<Response>(resultString);
                        //int.TryParse(responseData.message.content.Trim(), out int rating);
                        if (responseData != null && int.TryParse(responseData.message.content.Trim(), out int rating))
                        {
                            // Save the rating to the database
                            await _mediator.Send(new UpdateArticleRateCommand
                            {
                                ArticleId = articleDto.Id,
                                NewRating = rating
                            }, token);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Failed to rate article {ArticleId}: {StatusCode}", articleDto.Id,
                            response.StatusCode);
                    }
                }
            }
        }

        public async Task RateArticlesInParallelAsync(CancellationToken token = default)
        {
            var apiUrl = _configuration["AppSettings:OllamaApiUrl"];
            // Call the API using _mediator and return the result

            var articlesToRate = await _mediator.Send(new GetArticlesToRateQuery(), token);
            var concDict = new ConcurrentDictionary<Guid, int>();

            //try to parallel the requests to the llm api if you have powerful hardware
            
            await Parallel.ForEachAsync(articlesToRate, token, async (dto, token) => 
            {
                var articleText =
                    dto.Content;

                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                    request.Headers.Add("Accept", "application/json");


                    request.Content = JsonContent.Create(new Request()
                    {
                        model = "gpt-oss:20b",
                        messages = new List<Message>
                        {
                            new Message
                            {
                                role = "user",
                                content =
                                    $"Rate this article by positivity rate from -10 to 10 and provide only value as response. {Environment.NewLine} {articleText}"
                            }
                        },
                        stream = false
                    });

                    var response = await httpClient.SendAsync(request, token);
                    if (response.IsSuccessStatusCode)
                    {
                        var resultString = await response.Content.ReadAsStringAsync(token);
                        var responseData = JsonConvert.DeserializeObject<Response>(resultString);
                        //int.TryParse(responseData.message.content.Trim(), out int rating);
                        if (responseData != null && int.TryParse(responseData.message.content.Trim(), out int rating))
                        {
                            // Save the rating to the database
                            concDict.TryAdd(dto.Id, rating);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Failed to rate article {ArticleId}: {StatusCode}", dto.Id,
                            response.StatusCode);
                    }
                }
            });

            await _mediator.Send(new UpdateArticleRatesCommand
            {
                ArticleRatings = concDict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            }, token);
        }

    }
}