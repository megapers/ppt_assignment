using AvatarPicker.Services.Strategies;
using System.Text.RegularExpressions;

namespace AvatarPicker.UnitTest.Strategies;

[TestFixture]
public class NonAlphanumericStrategyTests
{
    private NonAlphanumericStrategy _strategy;

    [SetUp]
    public void Setup()
    {
        _strategy = new NonAlphanumericStrategy();
    }

    [TestCase("user@example")]
    [TestCase("test!")]
    [TestCase("example#123")]
    [TestCase("sample_text")]
    public void CanApply_ReturnsTrue_ForIdentifiersWithNonAlphanumericCharacters(string identifier)
    {
        Assert.That(_strategy.CanApply(identifier), Is.True);
    }

    [TestCase("user123")]
    [TestCase("TEST")]
    [TestCase("example")]
    public void CanApply_ReturnsFalse_ForIdentifiersWithoutNonAlphanumericCharacters(string identifier)
    {
        Assert.That(_strategy.CanApply(identifier), Is.False);
    }

    [Test]
    public async Task GetImageUrlAsync_ReturnsUrlWithRandomSeedBetween1And5()
    {
        var result = await _strategy.GetImageUrlAsync("any@identifier");
        var match = Regex.Match(result, @"seed=(\d)&size=150");
        
        Assert.That(match.Success, Is.True);
        var seedValue = int.Parse(match.Groups[1].Value);
        Assert.That(seedValue, Is.GreaterThanOrEqualTo(1).And.LessThanOrEqualTo(5));
    }
}