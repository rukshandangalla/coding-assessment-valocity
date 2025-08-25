using System;
using CodingAssessment.Refactor;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class PeopleTests
    {
        [Fact]
        public void People_DefaultConstructor_ShouldSetAgeToUnder16()
        {
            // Arrange & Act
            var person = new People("TestPerson");
            
            // Assert
            var expectedAge = DateTimeOffset.UtcNow.AddYears(-15);
            person.DOB.Date.Should().BeCloseTo(expectedAge.Date, TimeSpan.FromDays(1));
        }

        [Fact]
        public void People_WithDateConstructor_ShouldSetCorrectDOB()
        {
            // Arrange
            var testDate = new DateTime(1990, 5, 15);
            
            // Act
            var person = new People("John", testDate);
            
            // Assert
            person.Name.Should().Be("John");
            person.DOB.Should().Be(testDate);
        }
    }
}