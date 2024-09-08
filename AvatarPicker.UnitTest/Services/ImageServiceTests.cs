using Moq;

namespace AvatarPicker.UnitTest;

[TestFixture]
public class ImageServiceTests
{
    private Mock<IImageRetrievalStrategy> _mockStrategy;
    private ImageService _service;

    [SetUp]
    public void Setup()
    {
        _mockStrategy = new Mock<IImageRetrievalStrategy>();
        _service = new ImageService(_mockStrategy.Object);
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsStrategyResult_WhenStrategyApplies()
    {
        _mockStrategy.Setup(s => s.CanApply(It.IsAny<string>())).Returns(true);
        _mockStrategy.Setup(s => s.GetImageUrlAsync(It.IsAny<string>())).ReturnsAsync("https://test.com/image.png");

        var result = await _service.GetImageUrlAsync("user1");

        Assert.That("https://test.com/image.png", Is.EqualTo(result));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsDefaultUrl_WhenStrategyDoesNotApply()
    {
        _mockStrategy.Setup(s => s.CanApply(It.IsAny<string>())).Returns(false);

        var result = await _service.GetImageUrlAsync("user1");

        Assert.That("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150", Is.EqualTo(result));
    }
}