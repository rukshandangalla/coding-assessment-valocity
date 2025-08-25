using System;
using CodingAssessment.Refactor;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class PersonTests
    {
        [Fact]
        public void Person_DefaultConstructor_ShouldSetAgeToDefault16Years()
        {
            // Arrange & Act
            var person = new Person("TestPerson");
            
            // Assert - Fixed to use -16 years
            var expectedAge = DateTimeOffset.UtcNow.AddYears(-16);
            person.DOB.Date.Should().BeCloseTo(expectedAge.Date, TimeSpan.FromDays(1));
        }

        [Fact]
        public void Person_WithDateConstructor_ShouldSetCorrectDOB()
        {
            // Arrange
            var testDate = new DateTime(1990, 5, 15);
            
            // Act
            var person = new Person("John", testDate);
            
            // Assert
            person.Name.Should().Be("John");
            person.DOB.Should().Be(testDate);
        }
    }
}