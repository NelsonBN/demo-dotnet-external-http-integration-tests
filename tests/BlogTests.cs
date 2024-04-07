using Demo.Api;
using FluentAssertions;

namespace Demo.Api.Tests;

[Collection(nameof(CollectionIntegrationTests))]
public sealed class BlogTests
{
    private readonly IntegrationTestsFactory _factory;

    public BlogTests(IntegrationTestsFactory factory)
        => _factory = factory;


    [Fact]
    public async Task Should_Return_200_status_code_and_10_comments()
    {
        // Arrange && Act
        var act = await _factory.CreateClient()
            .GetAsync("/comments");


        // Assert
        act.Should()
           .Be200Ok()
           .And.Satisfy<IEnumerable<CommentDto>>(model =>
                model.Should().HaveCountGreaterThanOrEqualTo(3));
    }

    [Fact]
    public async Task Should_Return_200_status_code_and_2_commets_only_post_3()
    {
        // Arrange
        var postId = Random.Shared.Next(1, 100);
        postId = 3;


        // Act
        var act = await _factory.CreateClient()
            .GetAsync($"/comments/{postId}");


        // Assert
        act.Should()
           .Be200Ok()
           .And.Satisfy<IEnumerable<CommentDto>>(model => 
           {
               model.Should().HaveCountGreaterThanOrEqualTo(2);
               model.Should().OnlyContain(x => x.PostId == postId);
           });
    }
}
