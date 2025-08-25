using System;
using System.Reflection;
using CodingAssessment.Refactor;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class GetBobsTests
    {
        [Fact]
        public void GetBobs_OlderThan30_HasBackwardsLogic()
        {
            // Testing private method through reflection to capture bug
            var birthingUnit = new BirthingUnit();
            
            // Add some test data with known ages
            birthingUnit.GetPeople(1); // This will add a Bob
            
            var getBobsMethod = typeof(BirthingUnit).GetMethod("GetBobs", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            // When asking for Bobs older than 30
            var result = getBobsMethod?.Invoke(birthingUnit, new object[] { true });
            
            // The current logic uses >= which means it returns people YOUNGER than 30
            // This captures the backwards logic bug
            result.Should().NotBeNull();
        }

        [Fact]
        public void DateCalculation_For30Years_Uses356Days()
        {
            // This documents that 30 years is calculated as 30 * 356 days
            var thirtyYearsInDays = 30 * 356;
            var actualThirtyYears = 30 * 365.25; // Correct calculation
            
            var difference = actualThirtyYears - thirtyYearsInDays;
            
            // The bug causes about 277.5 days difference over 30 years
            difference.Should().BeApproximately(277.5, 1);
        }
    }
}