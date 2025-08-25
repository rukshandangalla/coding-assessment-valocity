using System;
using System.Collegctions.Generic; // REVIEW: Typo - should be "Collections" not "Collegctions". This will cause a compilation error.
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    public class People // REVIEW: Consider renaming to "Person" (singular) as this represents a single person, not multiple people.
    {
        private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15); // REVIEW: Variable name is misleading - this calculates 15 years ago, not 16. Also, this is computed at startup time, not dynamically.
        public string Name { get; private set; }
        public DateTimeOffset DOB { get; private set; }
        public People(string name) : this(name, Under16.Date) { }
        public People(string name, DateTime dob)
        { // REVIEW: Parameter type mismatch - property DOB is DateTimeOffset but parameter is DateTime. Consider using consistent types.
            Name = name; // REVIEW: No validation - should check for null/empty name
            DOB = dob; // REVIEW: Implicit conversion from DateTime to DateTimeOffset may lose timezone information
        }
    }

    public class BirthingUnit
    {
        /// <summary>
        /// MaxItemsToRetrieve
        /// </summary>
        private List<People> _people; // REVIEW: XML comment doesn't match the field - this is a list of people, not a max items count

        public BirthingUnit()
        {
            _people = new List<People>();
        }

        /// <summary>
        /// GetPeoples
        /// </summary>
        /// <param name="j"></param> // REVIEW: Parameter name mismatch - comment says "j" but actual parameter is "i"
        /// <returns>List<object></returns> // REVIEW: Return type mismatch - comment says List<object> but method returns List<People>
        public List<People> GetPeople(int i) // REVIEW: Parameter name "i" is not descriptive - consider "count" or "numberOfPeople"
        {
            for (int j = 0; j < i; j++)
            {
                try
                {
                    // Creates a dandon Name // REVIEW: Typo - "dandon" should be "random"
                    string name = string.Empty;
                    var random = new Random(); // REVIEW: Creating Random inside loop will produce predictable results if called rapidly. Move to class field.
                    if (random.Next(0, 1) == 0)
                    { // REVIEW: Random.Next(0, 1) always returns 0. Use Next(0, 2) to get 0 or 1.
                        name = "Bob";
                    }
                    else
                    {
                        name = "Betty";
                    }
                    // Adds new people to the list
                    _people.Add(new People(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0)))); // REVIEW: Bug - 356 should be 365 (days in a year). Also, this keeps adding to the same list - consider if you want to clear it first.
                }
                catch (Exception e) // REVIEW: Variable 'e' is declared but never used. Consider logging the original exception.
                {
                    // Dont think this should ever happen
                    throw new Exception("Something failed in user creation"); // REVIEW: Throwing generic Exception loses stack trace. Use throw new Exception("message", e) to preserve inner exception.
                }
            }
            return _people;
        }

        private IEnumerable<People> GetBobs(bool olderThan30) // REVIEW: Method is private but never used - consider removing if not needed
        {
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob"); // REVIEW: Logic bug - >= should be <= for "older than 30". Also, 356 should be 365 days.
        }

        public string GetMarried(People p, string lastName) // REVIEW: Consider null checks for parameters
        {
            if (lastName.Contains("test")) // REVIEW: Why special handling for "test"? This seems like test code that shouldn't be in production.
                return p.Name;
            if ((p.Name.Length + lastName).Length > 255) // REVIEW: Bug - should be (p.Name.Length + lastName.Length) not (p.Name.Length + lastName).Length
            {
                (p.Name + " " + lastName).Substring(0, 255); // REVIEW: Bug - result is not returned. Add "return" before this line.
            }

            return p.Name + " " + lastName;
        }
    }
}