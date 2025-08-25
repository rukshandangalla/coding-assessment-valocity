using System;
using System.Linq;
using CodingAssessment.Refactor;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class DateCalculationTests
    {
        [Fact]
        public void GetPeople_AgeCalculation_Uses356DaysPerYear()
        {
            // This test documents the bug where 356 days is used instead of 365
            var birthingUnit = new BirthingUnit();
            
            // Generate many people to check age ranges
            var people = birthingUnit.GetPeople(50);
            
            // All people should be between 18 and 85 years old (using 356 days)
            foreach (var person in people)
            {
                var ageInDays = (DateTime.UtcNow - person.DOB).TotalDays;
                var approximateAge = ageInDays / 356; // Using the buggy calculation
                
                approximateAge.Should().BeInRange(17.5, 85.5, 
                    "because code uses 356 days per year for age calculation");
            }
        }

        [Fact] 
        public void People_DefaultConstructor_UsesNegative15Years()
        {
            // Documents that Under16 variable is actually -15 years
            var person = new People("Test");
            
            var expectedAge = DateTimeOffset.UtcNow.AddYears(-15);
            var daysDifference = Math.Abs((person.DOB - expectedAge).TotalDays);
            
            daysDifference.Should().BeLessThan(2, "because Under16 is set to -15 years");
        }
    }
}