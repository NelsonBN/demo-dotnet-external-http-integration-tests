using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Demo.Api;

public class JsonPlaceholderOptions
{
    public string Url { get; set; } = default!;


    public class Setup : IConfigureOptions<JsonPlaceholderOptions>
    {
        public const string SECTION_NAME = "JsonPlaceholder";

        private readonly IConfiguration _configuration;

        public Setup(IConfiguration configuration)
            => _configuration = configuration;

        public void Configure(JsonPlaceholderOptions options)
            => _configuration.GetSection(SECTION_NAME).Bind(options);
    }
}
