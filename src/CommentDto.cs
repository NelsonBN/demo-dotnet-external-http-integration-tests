namespace Demo.Api;

public record CommentDto(
    int Id,
    int PostId,
    string Name,
    string Email,
    string Body);
