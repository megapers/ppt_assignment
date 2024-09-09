using Moq;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using AvatarPicker.Services.Strategies;

namespace AvatarPicker.UnitTest;

[TestFixture]
public class LastDigit6To9StrategyTests
{
    private Mock<IWebHostEnvironment> _mockEnvironment;
    private LastDigit6To9Strategy _strategy;

    [SetUp]
    public void Setup()
    {
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockEnvironment.Setup(e => e.ContentRootPath).Returns(TestContext.CurrentContext.TestDirectory);
        _strategy = new LastDigit6To9Strategy(_mockEnvironment.Object);

        var mockData = new { images = new[] { new { id = 6, url = "https://test.com/image6.png" } } };
        var json = JsonSerializer.Serialize(mockData);
        Directory.CreateDirectory(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data"));
        File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "db.json"), json);
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
        var result = await _strategy.GetImageUrlAsync("user6");
        Assert.That("https://test.com/image6.png", Is.EqualTo(result));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsDefaultUrl_ForNonExistentImage()
    {
        var result = await _strategy.GetImageUrlAsync("user7");
        Assert.That("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150", Is.EqualTo(result));
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the mock db.json file
        File.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "db.json"));
        Directory.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data"));
    }
}