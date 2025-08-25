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
        public void GetPerson_AgeCalculation_Uses365DaysPerYear()
        {
            // Fixed: Now uses correct 365 days per year
            var birthingUnit = new BirthingUnit();
            
            // Generate many people to check age ranges
            var people = birthingUnit.GetPeople(50);
            
            // All people should be between 18 and 85 years old (using 365 days)
            foreach (var person in people)
            {
                var ageInDays = (DateTime.UtcNow - person.DOB).TotalDays;
                var approximateAge = ageInDays / 365; // Using correct calculation
                
                approximateAge.Should().BeInRange(17.5, 85.5, 
                    "because code now uses 365 days per year for age calculation");
            }
        }

        [Fact] 
        public void Person_DefaultConstructor_UsesNegative16Years()
        {
            // Fixed: DefaultAge is now correctly -16 years
            var person = new Person("Test");
            
            var expectedAge = DateTimeOffset.UtcNow.AddYears(-16);
            var daysDifference = Math.Abs((person.DOB - expectedAge).TotalDays);
            
            daysDifference.Should().BeLessThan(2, "because DefaultAge is set to -16 years");
        }
    }
}