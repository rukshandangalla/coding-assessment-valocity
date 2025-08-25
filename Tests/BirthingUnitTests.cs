using System;
using System.Linq;
using CodingAssessment.Refactor;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class BirthingUnitTests
    {
        [Fact]
        public void GetPeople_ShouldReturnRequestedNumberOfPeople()
        {
            // Arrange
            var birthingUnit = new BirthingUnit();
            
            // Act
            var result = birthingUnit.GetPeople(3);
            
            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetPeople_ShouldAccumulatePeopleAcrossMultipleCalls()
        {
            // Arrange
            var birthingUnit = new BirthingUnit();
            
            // Act
            birthingUnit.GetPeople(2);
            var result = birthingUnit.GetPeople(3);
            
            // Assert - This captures the bug where people accumulate
            result.Should().HaveCount(5, "because GetPeople adds to existing list");
        }

        [Fact]
        public void GetPeople_ShouldOnlyGenerateBobOrBettyNames()
        {
            // Arrange
            var birthingUnit = new BirthingUnit();
            
            // Act
            var people = birthingUnit.GetPeople(20);
            
            // Assert
            people.Should().OnlyContain(p => p.Name == "Bob" || p.Name == "Betty");
        }

        [Fact]
        public void GetMarried_WithTestInLastName_ShouldReturnOnlyFirstName()
        {
            // Arrange
            var birthingUnit = new BirthingUnit();
            var person = new People("Alice");
            
            // Act
            var result = birthingUnit.GetMarried(person, "testing");
            
            // Assert - Captures current behavior
            result.Should().Be("Alice");
        }

        [Fact]
        public void GetMarried_WithNormalLastName_ShouldCombineNames()
        {
            // Arrange
            var birthingUnit = new BirthingUnit();
            var person = new People("Bob");
            
            // Act
            var result = birthingUnit.GetMarried(person, "Smith");
            
            // Assert
            result.Should().Be("Bob Smith");
        }

        [Fact]
        public void GetMarried_WithLongNames_ReturnsFullStringWithoutTruncation()
        {
            // Arrange
            var birthingUnit = new BirthingUnit();
            var person = new People(new string('A', 200));
            var lastName = new string('B', 100);
            
            // Act
            var result = birthingUnit.GetMarried(person, lastName);
            
            // Assert - Captures bug: substring result is not returned
            result.Should().Be(new string('A', 200) + " " + new string('B', 100));
            result.Length.Should().BeGreaterThan(255, "because truncation logic doesn't return the result");
        }

        [Fact]
        public void GetPeople_RandomNameGeneration_HasBugInRandomRange()
        {
            // This test documents that Random.Next(0,1) only returns 0
            // So it will ONLY generate "Bob", never "Betty"
            var birthingUnit = new BirthingUnit();
            
            var people = birthingUnit.GetPeople(100);
            
            // This captures the bug - all names will be "Bob"
            people.Should().OnlyContain(p => p.Name == "Bob");
        }
    }
}