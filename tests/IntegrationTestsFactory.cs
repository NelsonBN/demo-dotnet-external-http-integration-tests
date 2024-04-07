using Microsoft.AspNetCore.Mvc.Testing;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;

namespace Demo.Api.Tests;

public class IntegrationTestsFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const int HTTP_MOCK_PORT = 5000;
    private IContainer _container = default!;

    public IntegrationTestsFactory()
        => _container = new ContainerBuilder()
            .WithImage("natenho/mockaco")
            .WithCleanUp(true)
            .WithPortBinding(HTTP_MOCK_PORT, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(HTTP_MOCK_PORT)
                .AddCustomWaitStrategy(new ContainerHealthCheck()))
            .WithBindMount(Path.GetFullPath(
                "./ResponseMocks"),
                "/app/Mocks",
                AccessMode.ReadWrite)
            .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        Environment.SetEnvironmentVariable(
            $"{JsonPlaceholderOptions.Setup.SECTION_NAME}:{nameof(JsonPlaceholderOptions.Url)}",
            _getMockUrl(_container));
    }

    public async Task DisposeAsync()
        => await _container.StopAsync();

    private class ContainerHealthCheck : IWaitUntil
    {
        public async Task<bool> UntilAsync(IContainer container)
        {
            try
            {
                using var httpClient = new HttpClient { BaseAddress = new Uri(_getMockUrl(container)) };
                var result = await httpClient.GetStringAsync("/_mockaco/ready");
                return result?.Equals("Healthy") ?? false;
            }
            catch
            {
                return false;
            }
        }
    }

    private static string _getMockUrl(IContainer container)
        => $"http://localhost:{container.GetMappedPublicPort(HTTP_MOCK_PORT)}";
}

[CollectionDefinition(nameof(CollectionIntegrationTests))]
public sealed class CollectionIntegrationTests : ICollectionFixture<IntegrationTestsFactory> { }
