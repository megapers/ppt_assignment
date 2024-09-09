using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using AvatarPicker.Services.Strategies;
using AvatarPicker.Models;

namespace AvatarPicker.UnitTest.Strategies;

[TestFixture]
public class LastDigit6To9StrategyTests
{
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private HttpClient _httpClient;
    private LastDigit6To9Strategy _strategy;

    [SetUp]
    public void Setup()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _strategy = new LastDigit6To9Strategy(_httpClient);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }

    [TestCase("user6")]
    [TestCase("test7")]
    [TestCase("example8")]
    [TestCase("sample9")]
    public void CanApply_ReturnsTrue_ForValidIdentifiers(string identifier)
    {
        Assert.That(_strategy.CanApply(identifier), Is.True);
    }

    [TestCase("user5")]
    [TestCase("test0")]
    [TestCase("examplea")]
    public void CanApply_ReturnsFalse_ForInvalidIdentifiers(string identifier)
    {
        Assert.That(_strategy.CanApply(identifier), Is.False);
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsCorrectUrl_ForValidIdentifier()
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new Image { Id = 6, Url = "https://test.com/image6.png" }))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var result = await _strategy.GetImageUrlAsync("user6");
        Assert.That(result, Is.EqualTo("https://test.com/image6.png"));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsDefaultUrl_ForHttpRequestException()
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException());

        var result = await _strategy.GetImageUrlAsync("user7");
        Assert.That(result, Is.EqualTo("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150"));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsDefaultUrl_ForNonSuccessStatusCode()
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var result = await _strategy.GetImageUrlAsync("user8");
        Assert.That(result, Is.EqualTo("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150"));
    }
}