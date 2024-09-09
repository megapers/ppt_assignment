using Moq;
using Microsoft.EntityFrameworkCore;
using AvatarPicker.Data;
using AvatarPicker.Models;
using AvatarPicker.Services.Strategies;

namespace AvatarPicker.UnitTest.Strategies;

[TestFixture]
public class LastDigit1To5StrategyTests
{
    private Mock<AvatarDbContext> _mockDbContext;
    private LastDigit1To5Strategy _strategy;

    [SetUp]
    public void Setup()
    {
        _mockDbContext = new Mock<AvatarDbContext>();
        _strategy = new LastDigit1To5Strategy(_mockDbContext.Object);
    }

    [TestCase("user1")]
    [TestCase("test2")]
    [TestCase("example3")]
    [TestCase("sample4")]
    [TestCase("data5")]
    public void CanApply_ReturnsTrue_ForValidIdentifiers(string identifier)
    {
        Assert.That(_strategy.CanApply(identifier), Is.True);
    }

    [TestCase("user6")]
    [TestCase("test0")]
    [TestCase("examplea")]
    public void CanApply_ReturnsFalse_ForInvalidIdentifiers(string identifier)
    {
        Assert.That(_strategy.CanApply(identifier), Is.False);
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsCorrectUrl_ForValidIdentifier()
    {
        var mockSet = new Mock<DbSet<Image>>();
        var expectedImage = new Image { Id = 3, Url = "https://test.com/image3.png" };

        mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(expectedImage);

        _mockDbContext.Setup(c => c.Images).Returns(mockSet.Object);

        var result = await _strategy.GetImageUrlAsync("user3");
        Assert.That(result, Is.EqualTo("https://test.com/image3.png"));
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsDefaultUrl_ForNonExistentImage()
    {
        var mockSet = new Mock<DbSet<Image>>();

        mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((Image)null);

        _mockDbContext.Setup(c => c.Images).Returns(mockSet.Object);

        var result = await _strategy.GetImageUrlAsync("user2");
        Assert.That(result, Is.EqualTo("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150"));
    }
}