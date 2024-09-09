using AvatarPicker.Services.Strategies;

namespace AvatarPicker.UnitTest.Strategies;

[TestFixture]
public class ContainsVowelStrategyTests
{
    private ContainsVowelStrategy _strategy;

    [SetUp]
    public void Setup()
    {
        _strategy = new ContainsVowelStrategy();
    }

    [TestCase("user")]
    [TestCase("test")]
    [TestCase("example")]
    [TestCase("AEI")]
    [TestCase("OUaeiou")]
    [TestCase("a4")]
    [TestCase("e7")]
    [TestCase("i9")]
    public void CanApply_ReturnsTrue_ForIdentifiersWithVowels(string identifier)
    {
        Assert.That(_strategy.CanApply(identifier), Is.True);
    }

    [TestCase("xyz")]
    [TestCase("123")]
    [TestCase("_!@#")]
    public void CanApply_ReturnsFalse_ForIdentifiersWithoutVowels(string identifier)
    {
        Assert.That(_strategy.CanApply(identifier), Is.False);
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsCorrectUrl()
    {
        var result = await _strategy.GetImageUrlAsync("anyIdentifier");
        Assert.That(result, Is.EqualTo("https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150"));
    }
}