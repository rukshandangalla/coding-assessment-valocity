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
            var person = new Person("Alice");
            
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
            var person = new Person("Bob");
            
            // Act
            var result = birthingUnit.GetMarried(person, "Smith");
            
            // Assert
            result.Should().Be("Bob Smith");
        }

        [Fact]
        public void GetMarried_WithLongNames_NowReturnsTruncatedString()
        {
            // Arrange
            var birthingUnit = new BirthingUnit();
            var person = new Person(new string('A', 200));
            var lastName = new string('B', 100);
            
            // Act
            var result = birthingUnit.GetMarried(person, lastName);
            
            // Assert - Fixed: now returns truncated string
            result.Length.Should().Be(255, "because names are truncated to 255 characters");
            result.Should().StartWith(new string('A', 200) + " ");
        }

        [Fact]
        public void GetPeople_RandomNameGeneration_NowGeneratesBothNames()
        {
            // Fixed: Random.Next(0,2) now properly generates both names
            var birthingUnit = new BirthingUnit();
            
            var people = birthingUnit.GetPeople(100);
            
            // After fix: should have both Bob and Betty
            people.Should().Contain(p => p.Name == "Bob");
            people.Should().Contain(p => p.Name == "Betty");
        }
    }
}