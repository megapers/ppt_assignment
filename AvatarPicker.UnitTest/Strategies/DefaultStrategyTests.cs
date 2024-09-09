using AvatarPicker.Services.Strategies;

namespace AvatarPicker.UnitTest.Strategies;

[TestFixture]
public class DefaultStrategyTests
{
    private DefaultStrategy _strategy;

    [SetUp]
    public void Setup()
    {
        _strategy = new DefaultStrategy();
    }

    [TestCase("user123")]
    [TestCase("TEST")]
    [TestCase("example")]
    [TestCase("")]
    [TestCase("@#$%")]
    public void CanApply_AlwaysReturnsTrue(string identifier)
    {
        Assert.That(_strategy.CanApply(identifier), Is.True);
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsDefaultUrl()
    {
        var result = await _strategy.GetImageUrlAsync("anyIdentifier");
        Assert.That(result, Is.EqualTo("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150"));
    }
}