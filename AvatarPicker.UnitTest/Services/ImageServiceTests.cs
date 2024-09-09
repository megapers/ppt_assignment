using Moq;

namespace AvatarPicker.UnitTest.Services;

[TestFixture]
public class ImageServiceTests
{
    private Mock<IImageRetrievalStrategy> _mockStrategy6To9;
    private Mock<IImageRetrievalStrategy> _mockStrategy1To5;
    private ImageService _service;

    [SetUp]
    public void Setup()
    {
        _mockStrategy6To9 = new Mock<IImageRetrievalStrategy>();
        _mockStrategy1To5 = new Mock<IImageRetrievalStrategy>();
        _service = new ImageService(new List<IImageRetrievalStrategy> 
        { 
            _mockStrategy6To9.Object, 
            _mockStrategy1To5.Object 
        });
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsStrategy6To9Result_WhenApplicable()
    {
        _mockStrategy6To9.Setup(s => s.CanApply("user6")).Returns(true);
        _mockStrategy6To9.Setup(s => s.GetImageUrlAsync("user6")).ReturnsAsync("https://test.com/image6.png");
        _mockStrategy1To5.Setup(s => s.CanApply("user6")).Returns(false);

        var result = await _service.GetImageUrlAsync("user6");

        Assert.That(result, Is.EqualTo("https://test.com/image6.png"));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsStrategy1To5Result_WhenApplicable()
    {
        _mockStrategy6To9.Setup(s => s.CanApply("user3")).Returns(false);
        _mockStrategy1To5.Setup(s => s.CanApply("user3")).Returns(true);
        _mockStrategy1To5.Setup(s => s.GetImageUrlAsync("user3")).ReturnsAsync("https://test.com/image3.png");

        var result = await _service.GetImageUrlAsync("user3");

        Assert.That(result, Is.EqualTo("https://test.com/image3.png"));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsDefaultUrl_WhenNoStrategyApplies()
    {
        _mockStrategy6To9.Setup(s => s.CanApply("userX")).Returns(false);
        _mockStrategy1To5.Setup(s => s.CanApply("userX")).Returns(false);

        var result = await _service.GetImageUrlAsync("userX");

        Assert.That(result, Is.EqualTo("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150"));
    }

    [Test]
    public async Task GetImageUrlAsync_CallsStrategiesInOrder()
    {
        var callOrder = new List<string>();
        _mockStrategy6To9.Setup(s => s.CanApply(It.IsAny<string>()))
            .Returns(false)
            .Callback<string>(_ => callOrder.Add("Strategy6To9"));
        _mockStrategy1To5.Setup(s => s.CanApply(It.IsAny<string>()))
            .Returns(true)
            .Callback<string>(_ => callOrder.Add("Strategy1To5"));
        _mockStrategy1To5.Setup(s => s.GetImageUrlAsync(It.IsAny<string>()))
            .ReturnsAsync("https://test.com/image.png");

        await _service.GetImageUrlAsync("user2");

        Assert.That(callOrder, Is.EqualTo(new[] { "Strategy6To9", "Strategy1To5" }));
    }
}