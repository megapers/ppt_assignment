using Moq;

namespace AvatarPicker.UnitTest.Services;

[TestFixture]
public class ImageServiceTests
{
    private Mock<IImageRetrievalStrategy> _mockStrategyNonAlphanumeric;
    private Mock<IImageRetrievalStrategy> _mockStrategyVowel;
    private Mock<IImageRetrievalStrategy> _mockStrategy6To9;
    private Mock<IImageRetrievalStrategy> _mockStrategy1To5;
    private Mock<IImageRetrievalStrategy> _mockStrategyDefault;
    private ImageService _service;

    [SetUp]
    public void Setup()
    {
        _mockStrategyNonAlphanumeric = new Mock<IImageRetrievalStrategy>();
        _mockStrategyVowel = new Mock<IImageRetrievalStrategy>();
        _mockStrategy6To9 = new Mock<IImageRetrievalStrategy>();
        _mockStrategy1To5 = new Mock<IImageRetrievalStrategy>();
        _mockStrategyDefault = new Mock<IImageRetrievalStrategy>();
        _service = new ImageService(new List<IImageRetrievalStrategy> 
        { 
            _mockStrategyNonAlphanumeric.Object,
            _mockStrategyVowel.Object,
            _mockStrategy6To9.Object, 
            _mockStrategy1To5.Object,
            _mockStrategyDefault.Object
        });
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsNonAlphanumericResult_WhenApplicable()
    {
        _mockStrategyNonAlphanumeric.Setup(s => s.CanApply("user@6")).Returns(true);
        _mockStrategyNonAlphanumeric.Setup(s => s.GetImageUrlAsync("user@6")).ReturnsAsync("https://api.dicebear.com/8.x/pixel-art/png?seed=3&size=150");
        _mockStrategyVowel.Setup(s => s.CanApply("user@6")).Returns(true);
        _mockStrategy6To9.Setup(s => s.CanApply("user@6")).Returns(true);
        _mockStrategy1To5.Setup(s => s.CanApply("user@6")).Returns(false);

        var result = await _service.GetImageUrlAsync("user@6");

        Assert.That(result, Does.Match(@"https://api\.dicebear\.com/8\.x/pixel-art/png\?seed=[1-5]&size=150"));
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
    public async Task GetImageUrlAsync_ReturnsDefaultResult_WhenNoOtherStrategyApplies()
    {
        _mockStrategyNonAlphanumeric.Setup(s => s.CanApply("xyz")).Returns(false);
        _mockStrategyVowel.Setup(s => s.CanApply("xyz")).Returns(false);
        _mockStrategy6To9.Setup(s => s.CanApply("xyz")).Returns(false);
        _mockStrategy1To5.Setup(s => s.CanApply("xyz")).Returns(false);
        _mockStrategyDefault.Setup(s => s.CanApply("xyz")).Returns(true);
        _mockStrategyDefault.Setup(s => s.GetImageUrlAsync("xyz")).ReturnsAsync("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150");

        var result = await _service.GetImageUrlAsync("xyz");

        Assert.That(result, Is.EqualTo("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150"));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsDefaultUrl_WhenNoStrategyApplies()
    {
        _mockStrategyNonAlphanumeric.Setup(s => s.CanApply("xyz")).Returns(false);
        _mockStrategyVowel.Setup(s => s.CanApply("xyz")).Returns(false);
        _mockStrategy6To9.Setup(s => s.CanApply("xyz")).Returns(false);
        _mockStrategy1To5.Setup(s => s.CanApply("xyz")).Returns(false);
        _mockStrategyDefault.Setup(s => s.CanApply("xyz")).Returns(false); // Change this to false

        var result = await _service.GetImageUrlAsync("xyz");

        Assert.That(result, Is.EqualTo("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150"));
    }

    [Test]
    public async Task GetImageUrlAsync_CallsStrategiesInOrder()
    {
        var callOrder = new List<string>();
        _mockStrategyNonAlphanumeric.Setup(s => s.CanApply(It.IsAny<string>()))
            .Returns(false)
            .Callback<string>(_ => callOrder.Add("StrategyNonAlphanumeric"));
        _mockStrategyVowel.Setup(s => s.CanApply(It.IsAny<string>()))
            .Returns(false)
            .Callback<string>(_ => callOrder.Add("StrategyVowel"));
        _mockStrategy6To9.Setup(s => s.CanApply(It.IsAny<string>()))
            .Returns(false)
            .Callback<string>(_ => callOrder.Add("Strategy6To9"));
        _mockStrategy1To5.Setup(s => s.CanApply(It.IsAny<string>()))
            .Returns(false)
            .Callback<string>(_ => callOrder.Add("Strategy1To5"));
        _mockStrategyDefault.Setup(s => s.CanApply(It.IsAny<string>()))
            .Returns(true)
            .Callback<string>(_ => callOrder.Add("StrategyDefault"));
        _mockStrategyDefault.Setup(s => s.GetImageUrlAsync(It.IsAny<string>()))
            .ReturnsAsync("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150");

        await _service.GetImageUrlAsync("xyz");

        Assert.That(callOrder, Is.EqualTo(new[] { "StrategyNonAlphanumeric", "StrategyVowel", "Strategy6To9", "Strategy1To5", "StrategyDefault" }));
    }
}