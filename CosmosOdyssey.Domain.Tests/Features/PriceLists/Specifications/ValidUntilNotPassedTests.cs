using CosmosOdyssey.Domain.Features.PriceLists;
using CosmosOdyssey.Domain.Features.PriceLists.Specifications;
using FizzWare.NBuilder;
using FluentAssertions;

namespace CosmosOdyssey.Domain.Tests.Features.PriceLists.Specifications;

public class ValidUntilNotPassedTests
{
    [Test]
    public void ToExpression_Should_Filter_PriceLists_Correctly()
    {
        // Arrange
        var validDate = DateTime.UtcNow;
        var priceLists = new List<PriceList>
        {
            Builder<PriceList>.CreateNew().With(x => x.ValidUntil, DateTime.Now.AddDays(1)).Build(), // Valid
            Builder<PriceList>.CreateNew().With(x => x.ValidUntil, DateTime.Now.AddDays(-1)).Build(), // Expired
            Builder<PriceList>.CreateNew().With(x => x.ValidUntil, DateTime.Now.AddDays(2)).Build() // Valid
        };

        var specification = new ValidUntilNotPassed(validDate);

        // Act
        var expression = specification.ToExpression();
        var result = priceLists.AsQueryable().Where(expression).ToArray();

        // Assert
        result.Length.Should().Be(2);
        result.Should().BeEquivalentTo(new[] { priceLists[0], priceLists[2] });
    }

    [Test]
    public void ToExpression_Should_Return_Empty_If_No_Matching_PriceLists()
    {
        // Arrange
        var validDate = DateTime.UtcNow;
        var priceLists = new List<PriceList>
        {
            Builder<PriceList>.CreateNew().With(x => x.ValidUntil, DateTime.Now.AddDays(-3)).Build(), // Expired
            Builder<PriceList>.CreateNew().With(x => x.ValidUntil, DateTime.Now.AddDays(-1)).Build() // Expired
        };

        var specification = new ValidUntilNotPassed(validDate);

        // Act
        var expression = specification.ToExpression();
        var result = priceLists.AsQueryable().Where(expression).ToArray();

        // Assert
        result.Should().BeEmpty();
    }
}