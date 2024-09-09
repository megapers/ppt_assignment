using Moq;
using AvatarPicker.Services.Strategies;

namespace AvatarPicker.UnitTest.Services;

[TestFixture]
public class ImageServiceTests
{
    private Mock<IImageRetrievalStrategy> _mockStrategyVowel;
    private Mock<IImageRetrievalStrategy> _mockStrategy6To9;
    private Mock<IImageRetrievalStrategy> _mockStrategy1To5;
    private ImageService _service;

    [SetUp]
    public void Setup()
    {
        _mockStrategyVowel = new Mock<IImageRetrievalStrategy>();
        _mockStrategy6To9 = new Mock<IImageRetrievalStrategy>();
        _mockStrategy1To5 = new Mock<IImageRetrievalStrategy>();
        _service = new ImageService(new List<IImageRetrievalStrategy> 
        { 
            _mockStrategyVowel.Object,
            _mockStrategy6To9.Object, 
            _mockStrategy1To5.Object 
        });
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsVowelResult_WhenApplicable()
    {
        _mockStrategyVowel.Setup(s => s.CanApply("user6")).Returns(true);
        _mockStrategyVowel.Setup(s => s.GetImageUrlAsync("user6")).ReturnsAsync("https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150");
        _mockStrategy6To9.Setup(s => s.CanApply("user6")).Returns(true);
        _mockStrategy1To5.Setup(s => s.CanApply("user6")).Returns(false);

        var result = await _service.GetImageUrlAsync("user6");

        Assert.That(result, Is.EqualTo("https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150"));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsStrategy6To9Result_WhenNoVowelAndApplicable()
    {
        _mockStrategyVowel.Setup(s => s.CanApply("xyz6")).Returns(false);
        _mockStrategy6To9.Setup(s => s.CanApply("xyz6")).Returns(true);
        _mockStrategy6To9.Setup(s => s.GetImageUrlAsync("xyz6")).ReturnsAsync("https://test.com/image6.png");
        _mockStrategy1To5.Setup(s => s.CanApply("xyz6")).Returns(false);

        var result = await _service.GetImageUrlAsync("xyz6");

        Assert.That(result, Is.EqualTo("https://test.com/image6.png"));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsStrategy1To5Result_WhenNoVowelAndApplicable()
    {
        _mockStrategyVowel.Setup(s => s.CanApply("xyz3")).Returns(false);
        _mockStrategy6To9.Setup(s => s.CanApply("xyz3")).Returns(false);
        _mockStrategy1To5.Setup(s => s.CanApply("xyz3")).Returns(true);
        _mockStrategy1To5.Setup(s => s.GetImageUrlAsync("xyz3")).ReturnsAsync("https://test.com/image3.png");

        var result = await _service.GetImageUrlAsync("xyz3");

        Assert.That(result, Is.EqualTo("https://test.com/image3.png"));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsDefaultUrl_WhenNoStrategyApplies()
    {
        _mockStrategyVowel.Setup(s => s.CanApply("xyz")).Returns(false);
        _mockStrategy6To9.Setup(s => s.CanApply("xyz")).Returns(false);
        _mockStrategy1To5.Setup(s => s.CanApply("xyz")).Returns(false);

        var result = await _service.GetImageUrlAsync("xyz");

        Assert.That(result, Is.EqualTo("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150"));
    }

    [Test]
    public async Task GetImageUrlAsync_CallsStrategiesInOrder()
    {
        var callOrder = new List<string>();
        _mockStrategyVowel.Setup(s => s.CanApply(It.IsAny<string>()))
            .Returns(false)
            .Callback<string>(_ => callOrder.Add("StrategyVowel"));
        _mockStrategy6To9.Setup(s => s.CanApply(It.IsAny<string>()))
            .Returns(true)
            .Callback<string>(_ => callOrder.Add("Strategy6To9"));
        _mockStrategy6To9.Setup(s => s.GetImageUrlAsync(It.IsAny<string>()))
            .ReturnsAsync("https://test.com/image.png");
        _mockStrategy1To5.Setup(s => s.CanApply(It.IsAny<string>()))
            .Returns(false)
            .Callback<string>(_ => callOrder.Add("Strategy1To5"));

        await _service.GetImageUrlAsync("xyz6");

        Assert.That(callOrder, Is.EqualTo(new[] { "StrategyVowel", "Strategy6To9" }));
    }
}