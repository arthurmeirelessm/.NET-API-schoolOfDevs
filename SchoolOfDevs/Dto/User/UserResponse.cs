using SchoolOfDevs.Entities;
using SchoolOfDevs.Enuns;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolOfDevs.Dto.User
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string UserName { get; set; }
        public TypeUser TypeUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public List<Course> CoursesStuding { get; set; } // typeUser == Student
        public List<Course> CoursesTeaching { get; set; } // typeUser == Teacher
    }
}
