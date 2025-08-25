using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingAssessment.Refactor
{
    public class Person
    {
        private static readonly DateTimeOffset DefaultAge = DateTimeOffset.UtcNow.AddYears(-16);  // Default age for a person
        public string Name { get; private set; }
        public DateTimeOffset DOB { get; private set; }

        public Person(string name) : this(name, DefaultAge.Date)
        {
        }

        public Person(string name, DateTime dob)
        {
            Name = name;
            DOB = dob;
        }
    }

    public class BirthingUnit
    {
        /// <summary>
        /// List of generated people
        /// </summary>
        private List<Person> _people;

        public BirthingUnit()
        {
            _people = new List<Person>();
        }

        /// <summary>
        /// Generate people and add to internal list
        /// </summary>
        /// <param name="i">Number of people to generate</param>
        /// <returns>List of all people (including previously generated)</returns>
        public List<Person> GetPeople(int i)
        {
            for (int j = 0; j < i; j++)
            {
                try
                {
                    // Creates a random Name
                    string name = string.Empty;
                    var random = new Random();
                    if (random.Next(0, 2) == 0)
                    {  // Fixed: was Next(0,1) which only returns 0
                        name = "Bob";
                    }
                    else
                    {
                        name = "Betty";
                    }
                    // Adds new people to the list
                    _people.Add(new Person(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 365, 0, 0, 0))));
                }
                catch (Exception ex)
                {
                    // Dont think this should ever happen
                    throw new Exception("Something failed in user creation", ex);
                }
            }
            return _people;
        }

        private IEnumerable<Person> GetBobs(bool olderThan30)
        {
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB < DateTime.Now.Subtract(new TimeSpan(30 * 365, 0, 0, 0))) : _people.Where(x => x.Name == "Bob");
        }

        public string GetMarried(Person p, string lastName)
        {
            if (lastName.Contains("test"))
                return p.Name;
            if ((p.Name.Length + lastName.Length) > 255)
            {
                return (p.Name + " " + lastName).Substring(0, 255);
            }

            return p.Name + " " + lastName;
        }
    }
}