using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Api;

public class JsonPlaceholderService(HttpClient client)
{
    private readonly HttpClient _client = client;

    public async Task<IEnumerable<CommentDto>?> GetCommentsAsync(CancellationToken cancellationToken = default)
    {
        var comments = await _client.GetFromJsonAsync<IEnumerable<CommentDto>>("comments", cancellationToken);
        return comments;
    }

    public async Task<IEnumerable<CommentDto>?> GetCommentAsync(int id, CancellationToken cancellationToken = default)
    {
        var comments = await _client.GetFromJsonAsync<IEnumerable<CommentDto>>($"comments?postId={id}", cancellationToken);
        return comments;
    }
}
