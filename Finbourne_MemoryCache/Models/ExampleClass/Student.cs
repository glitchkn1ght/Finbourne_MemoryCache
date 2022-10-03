using System;
using System.Collections.Generic;
using System.Text;

namespace Finbourne_MemoryCache.Models.ExampleClass
{
    //Just to show the cache works with more complex objects. 
    public class Student
    {
        public Student(int id, string firstName, string lastName)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
        
        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }    
    }
}
